using Microsoft.VisualBasic;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System.ComponentModel;

namespace Later.App;

internal class WasapiAudioCaptureDevice : IAudioCaptureDevice<WaveFormat, WaveInEventArgs, StoppedEventArgs>
{
    private readonly WasapiCapture _capture;


    public event EventHandler<WaveInEventArgs>? DataAvailable;
    public event EventHandler<StoppedEventArgs>? CaptureStopped;

    public WasapiAudioCaptureDevice(WasapiCapture wasapiCapture)
    {
        _capture = wasapiCapture;
        _capture.DataAvailable += OnDataAvailable;
        _capture.RecordingStopped += OnRecordingStopped;
    }

    public WasapiAudioCaptureDevice(MMDevice device, bool useEventSync = true, int audioBufferMillisecondsLength = 10)
        : this(new WasapiCapture(device, useEventSync, audioBufferMillisecondsLength)) 
    {
        device.AudioEndpointVolume.MasterVolumeLevelScalar = 1f;
    }

    private WasapiAudioCaptureDevice(MMDeviceEnumerator enumerator, string? deviceID = null, bool useEventSync = true, int audioBufferMillisecondsLength = 10)
        : this(deviceID == null
              ? enumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Multimedia)
              : enumerator.GetDevice(deviceID),
              useEventSync,
              audioBufferMillisecondsLength)
    {
        enumerator.Dispose();
    }

    public WasapiAudioCaptureDevice(string deviceId, bool useEventSync = true, int audioBufferMillisecondsLength = 10)
        : this(new MMDeviceEnumerator(), deviceId, useEventSync, audioBufferMillisecondsLength)
    {
    }

    public WasapiAudioCaptureDevice()
        : this(new MMDeviceEnumerator())
    {
    }

    public CaptureState CaptureState => _capture.CaptureState switch
    {
        NAudio.CoreAudioApi.CaptureState.Starting => CaptureState.Starting,
        NAudio.CoreAudioApi.CaptureState.Capturing => CaptureState.Capturing,
        NAudio.CoreAudioApi.CaptureState.Stopping => CaptureState.Stopping,
        NAudio.CoreAudioApi.CaptureState.Stopped => CaptureState.Stopped,
        _ => throw new NotImplementedException($"Unknown state '{_capture.CaptureState}'.")
    };

    public WaveFormat AudioFormat => _capture.WaveFormat;

    public async Task StartCapturingAsync()
    {
        _capture.StartRecording();

        while (_capture.CaptureState == NAudio.CoreAudioApi.CaptureState.Starting)
        {
            await Task.Delay(20).ConfigureAwait(false);
        }
    }

    public async Task StopCapturingAsync()
    {
        _capture.StopRecording();

        while (_capture.CaptureState == NAudio.CoreAudioApi.CaptureState.Stopping)
        {
            await Task.Delay(20).ConfigureAwait(false);
        }
    }

    protected virtual void OnDataAvailable(object? sender, WaveInEventArgs e)
    {
        DataAvailable?.Invoke(sender, e);
    }

    protected void OnRecordingStopped(object? sender, StoppedEventArgs e)
    {
        CaptureStopped?.Invoke(this, e);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            DataAvailable = null;
            CaptureStopped = null;
            if (_capture.CaptureState != NAudio.CoreAudioApi.CaptureState.Stopped)
            {
                _capture.StopRecording();
            }
            _capture.Dispose();
        }
    }
}
