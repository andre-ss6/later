using Microsoft.VisualBasic;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Later.App;

internal partial class MainForm : Form
{
    private const int _rollingAverageWindow = 5;
    private readonly List<double> _latencySamples = [];

    public MainForm()
    {
        InitializeComponent();
        Initialize();
    }

    private DateTimeOffset? PlayedAt { get; set; }
    private DateTimeOffset? ClickedAt { get; set; }
    private DateTimeOffset? DetectedAt { get; set; }
    private WasapiAudioCaptureDevice? CaptureDevice { get; set; }
    private WasapiOut? OutDevice { get; set; }
    private AnalysisState CurrentAnalysis { get; set; }

    private void Initialize()
    {
        audioModeCombobox.SelectedIndex = 0;

        using var enumerator = new MMDeviceEnumerator();
        var defaultCaptureDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Multimedia);
        var defaultOutputDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

        inputDevicesComboBox.SelectedIndexChanged += (sender, e) =>
        {
            micLatencyNumUpDown.Value = 0;
        };

        var endpoints = enumerator.EnumerateAudioEndPoints(DataFlow.All, DeviceState.Active);

        inputDevicesComboBox.Items.AddRange([.. endpoints.Where(e => e.DataFlow == DataFlow.Capture).Select(e =>
        {
            try
            {
                return new AudioEndpointComboboxItem { DeviceId = e.ID, DeviceName = e.FriendlyName };
            }
            finally
            {
                e.Dispose();
            }
        })]);
        inputDevicesComboBox.SelectedItem = inputDevicesComboBox.Items.Cast<AudioEndpointComboboxItem>().Single(d => d.DeviceId == defaultCaptureDevice.ID);

        outputDevicesCombobox.Items.AddRange([.. endpoints.Where(e => e.DataFlow == DataFlow.Render).Select(e =>
        {
            try
            {
                return new AudioEndpointComboboxItem { DeviceId = e.ID, DeviceName= e.FriendlyName };
            }
            finally
            {
                e.Dispose();
            }
        })]);
        outputDevicesCombobox.SelectedItem = outputDevicesCombobox.Items.Cast<AudioEndpointComboboxItem>().Single(d => d.DeviceId == defaultOutputDevice.ID);
    }

    [MemberNotNull(nameof(CaptureDevice))]
    private void InitializeCaptureDevice(string deviceId)
    {
        DisposeCaptureDevice();
        CaptureDevice = new NoiseGateFilter(deviceId, true, 10);
        CaptureDevice.CaptureStopped += CaptureDevice_CaptureStopped;
        ((IAudioFilter<NoiseGateFilteredWaveInEventArgs>)CaptureDevice).DataAvailable += CaptureDevice_DataAvailable;
    }

    [MemberNotNullWhen(true, nameof(OutDevice))]
    private bool InitializeOutputDevice(string deviceId)
    {
        DisposeOutputDevice();

        using var enumerator = new MMDeviceEnumerator();
        var device = enumerator.GetDevice(deviceId);

        var shareMode = (AudioClientShareMode)audioModeCombobox.SelectedIndex;
        var format = device.AudioClient.MixFormat;

        bool exclusiveModeUnsupported = false;
        try
        {
            // There's currently a bug on NAudio where calling IsFormatSupported
            // with shareMode = exclusive on a device that does not allow exclusive mode
            // throws an exception.
            // Also, we don't test for formats in general because this function is unreliable
            // (it'll return false even when the format works perfectly fine if you Init with it)
            device.AudioClient.IsFormatSupported(shareMode, format);
        }
        catch (COMException e) when (e.ErrorCode == -2004287474) // AUDCLNT_E_EXCLUSIVE_MODE_NOT_ALLOWED 
        {
            exclusiveModeUnsupported = true;
        }


        if (exclusiveModeUnsupported)
        {
            MessageBox.Show("This device does not allow Exclusive mode.");
            device.Dispose();
            return false;
        }

        OutDevice = new WasapiOut(device, shareMode, true, 5);

        OutDevice.PlaybackStopped += OutDevice_PlaybackStopped;

        OutDevice.Init(GenerateSineWaveSound(format));

        return true;

        ISampleProvider GenerateSineWaveSound(WaveFormat format)
        {
            var sg = new SignalGenerator(format.SampleRate, 2)
            {
                Gain = 0.3 * (shareMode == AudioClientShareMode.Shared ? 1 : device.AudioEndpointVolume.MasterVolumeLevelScalar),
                Frequency = 1000,
                Type = SignalGeneratorType.Sin,
            }.Take(TimeSpan.FromSeconds(5));

            return sg;
        }
    }

    private async Task StartRecording()
    {
        if (inputDevicesComboBox.SelectedItem == null)
        {
            return;
        }

        _latencySamples.Clear();

        if (micLatencyNumUpDown.Value > 0)
        {
            CurrentAnalysis = AnalysisState.OutputLatency;
            startOutputButton.Enabled = true;
        }
        else
        {
            CurrentAnalysis = AnalysisState.InputLatency;
            clickLatencyButton.Enabled = true;
        }
        startRecordingButton.BackColor = Color.Red;
        startRecordingButton.ForeColor = Color.Black;
        inputDevicesComboBox.Enabled = false;
        mouseLatencyNumUpDown.Enabled = false;
        outputDevicesCombobox.Enabled = false;
        audioModeCombobox.Enabled = false;
        micLatencyNumUpDown.Enabled = false;

        InitializeCaptureDevice(((AudioEndpointComboboxItem)inputDevicesComboBox.SelectedItem).DeviceId);
        await CaptureDevice.StartCapturingAsync();
    }

    private void StopRecording()
    {
        DisposeCaptureDevice();
        DisposeOutputDevice();

        volumeMeterProgressBar.Value = 0;
        startOutputButton.Enabled = false;
        clickLatencyButton.Enabled = false;
        startRecordingButton.BackColor = Color.White;
        startRecordingButton.ForeColor = Color.Red;
        inputDevicesComboBox.Enabled = true;
        mouseLatencyNumUpDown.Enabled = true;
        outputDevicesCombobox.Enabled = true;
        audioModeCombobox.Enabled = true;
        micLatencyNumUpDown.Enabled = true;
    }

    private void CaptureDevice_DataAvailable(object? sender, NoiseGateFilteredWaveInEventArgs e)
    {
        BeginInvoke(new Action(() =>
        {
            float percentage = 100 * e.MaxSample;
            volumeMeterProgressBar.Value = (int)percentage;
            volumeMeterProgressBar.ForeColor = VolumeColorInterpolation.ValueToBiasedHeatColor(percentage);

            if (percentage < 10)
            {
                return;
            }

            if (CurrentAnalysis == AnalysisState.InputLatency && DateTimeOffset.UtcNow > ClickedAt && DetectedAt == null)
            {
                DetectedAt = DateTimeOffset.UtcNow;

                var latency = (DetectedAt - ClickedAt).Value.TotalMilliseconds;
                if (_latencySamples.Count == _rollingAverageWindow)
                {
                    _latencySamples.RemoveAt(0);
                }
                _latencySamples.Add(latency);

                micLastDetectionLatencyLabel.Text = $"Last Detection Latency: {latency:#.#}ms";
                var avgLatency = _latencySamples.Average();
                micAverageLatencyLabel.Text = $"Microphone Average Latency: {avgLatency:#.#}ms";
                micLatencyNumUpDown.Value = (decimal)avgLatency;
                ClickedAt = null;
            }
            else if (CurrentAnalysis == AnalysisState.OutputLatency && DateTimeOffset.UtcNow > PlayedAt && DetectedAt == null)
            {
                DetectedAt = DateTimeOffset.UtcNow;

                var latency = (DetectedAt - (PlayedAt + TimeSpan.FromMilliseconds((long)micLatencyNumUpDown.Value))).Value.TotalMilliseconds;
                if (_latencySamples.Count == _rollingAverageWindow)
                {
                    _latencySamples.RemoveAt(0);
                }
                _latencySamples.Add(latency);

                outLastDetectionLatencyLabel.Text = $"Last Detection Latency: {latency:#.#}ms";
                var avgLatency = _latencySamples.Average();
                outAverageLatencyLabel.Text = $"Output Average Latency: {avgLatency:#.#}ms";
                PlayedAt = null;
            }
        }));
    }

    private void CaptureDevice_CaptureStopped(object? sender, StoppedEventArgs e)
    {
        BeginInvoke(new Action(() =>
        {
            if (CaptureDevice != null)
            {
                ((IAudioFilter<NoiseGateFilteredWaveInEventArgs>)CaptureDevice).DataAvailable -= CaptureDevice_DataAvailable;
            }

            StopRecording();
            if (e.Exception != null)
            {
                MessageBox.Show($"An error occurred during capture: {e.Exception.Message}", "An error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }));
    }

    private void OutDevice_PlaybackStopped(object? sender, StoppedEventArgs e)
    {
        BeginInvoke(new Action(() =>
        {
            if (e.Exception != null)
            {
                DisposeOutputDevice();
                MessageBox.Show($"An error occurred during playback: {e.Exception.Message}.", "Playback error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }));
    }

    private async void StartRecordingButton_Click(object sender, EventArgs e)
    {
        PlayedAt = null;
        ClickedAt = null;
        DetectedAt = null;

        if (CaptureDevice == null || CaptureDevice.CaptureState == CaptureState.Stopped)
        {
            await StartRecording();
        }
        else if (CaptureDevice.CaptureState == CaptureState.Capturing)
        {
            StopRecording();
        }
    }

    private void ClickLatencyButton_MouseDown(object sender, MouseEventArgs e)
    {
        clickLatencyButton.Enabled = false;
        DetectedAt = null;
        ClickedAt = DateTimeOffset.UtcNow - TimeSpan.FromMilliseconds((long)mouseLatencyNumUpDown.Value);
        _ = Task.Delay(300).ContinueWith(t => clickLatencyButton.Invoke(() => clickLatencyButton.Enabled = true), TaskScheduler.Current);
    }

    private async void StartOutputButton_Click(object sender, EventArgs e)
    {
        if (outputDevicesCombobox.SelectedItem == null)
        {
            return;
        }

        startOutputButton.Enabled = false;

        if (InitializeOutputDevice(((AudioEndpointComboboxItem)outputDevicesCombobox.SelectedItem).DeviceId))
        {
            do
            {
                DetectedAt = null;
                OutDevice?.Stop();
                await Task.Delay(1500);
                OutDevice?.Play();
                PlayedAt = DateTimeOffset.UtcNow;
                await Task.Delay(500);
            }
            while (OutDevice != null && OutDevice.PlaybackState != PlaybackState.Stopped);
        }

        StopRecording();
    }

    private void DisposeCaptureDevice()
    {
        if (CaptureDevice != null)
        {
            ((IAudioFilter<NoiseGateFilteredWaveInEventArgs>)CaptureDevice).DataAvailable -= CaptureDevice_DataAvailable;
            CaptureDevice.CaptureStopped -= CaptureDevice_CaptureStopped;

            if (CaptureDevice.CaptureState != CaptureState.Stopped)
            {
                _ = CaptureDevice.StopCapturingAsync();
            }

            CaptureDevice.Dispose();
        }
    }

    private void DisposeOutputDevice()
    {
        if (OutDevice != null)
        {
            var outDevice = OutDevice;
            OutDevice = null;
            outDevice.PlaybackStopped -= OutDevice_PlaybackStopped;
            outDevice.Stop();
            outDevice.Dispose();
        }
    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
            DisposeCaptureDevice();
            DisposeOutputDevice();
        }

        base.Dispose(disposing);
    }

    private sealed class AudioEndpointComboboxItem
    {
        public required string DeviceId { get; init; }
        public required string DeviceName { get; init; }

        public override string ToString()
        {
            return DeviceName;
        }
    }

    private enum AnalysisState
    {
        InputLatency,
        OutputLatency
    }
}
