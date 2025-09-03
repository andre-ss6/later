using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace Later.App;

internal class NoiseGateFilteredWaveInEventArgs : WaveInEventArgs
{
    public NoiseGateFilteredWaveInEventArgs(byte[] buffer, int bytes) : base(buffer, bytes)
    {
    }

    public float MaxSample { get; init; }
}

internal class NoiseGateFilter : WasapiAudioCaptureDevice, IAudioFilter<NoiseGateFilteredWaveInEventArgs>
{
    private float[]? _dataBuffer;
    private readonly FilterState[] _filterStates;
    private EventHandler<NoiseGateFilteredWaveInEventArgs>? _dataAvailableSubscribers;

    private readonly float thresholdLinear;   // linear amplitude threshold
    private readonly float attackAlpha;       // for envelope when signal rises
    private readonly float releaseAlpha;      // for envelope when signal falls
    private readonly float attackGainAlpha;   // for gain smoothing when opening
    private readonly float releaseGainAlpha;  // for gain smoothing when closing
    private readonly int holdSamples;         // number of samples to hold open

    // thresholdDb: negative number typically (e.g. -40 dB)
    // attackMs/releaseMs: envelope/gain smoothing times in milliseconds
    // holdMs: keep gate open for holdMs after envelope falls below threshold
    public NoiseGateFilter(
        string deviceId,
        bool useEventSync = false,
        int audioBufferMillisecondsLength = 100,
        float thresholdDb = -20f,
        float attackMs = 5f,
        float releaseMs = 150f,
        float holdMs = 40f) : base(deviceId, useEventSync, audioBufferMillisecondsLength)
    {
        var sampleRate = AudioFormat.SampleRate;
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(sampleRate);
        thresholdLinear = (float)Math.Pow(10.0, thresholdDb / 20.0);

        // Convert times to per-sample smoothing coefficients.
        // alpha = exp(-1 / (timeSec * sampleRate)). Smaller time -> faster response.
        double attackSec = Math.Max(1e-4, attackMs / 1000.0);
        double releaseSec = Math.Max(1e-4, releaseMs / 1000.0);

        attackAlpha = (float)Math.Exp(-1.0 / (attackSec * sampleRate));
        releaseAlpha = (float)Math.Exp(-1.0 / (releaseSec * sampleRate));

        // Use slightly faster attack for gain ramping to reduce audible artifacts.
        attackGainAlpha = (float)Math.Exp(-1.0 / (Math.Max(1e-4, attackMs / 2.0) * sampleRate / 1000.0));
        releaseGainAlpha = releaseAlpha;

        holdSamples = Math.Max(0, (int)Math.Round(holdMs * sampleRate / 1000.0));

        _filterStates = new FilterState[AudioFormat.Channels];
        for(int i =0; i < _filterStates.Length; i++)
        {
            _filterStates[i] = new FilterState();
            _filterStates[i].Reset(startOpen: true);
        }
    }

    event EventHandler<NoiseGateFilteredWaveInEventArgs> IAudioSampleProvider<NoiseGateFilteredWaveInEventArgs>.DataAvailable
    {
        add
        {
            if (_dataAvailableSubscribers == null)
            {
                _dataAvailableSubscribers = value;
            }
            else
            {
                _dataAvailableSubscribers = (EventHandler<NoiseGateFilteredWaveInEventArgs>)Delegate.Combine(_dataAvailableSubscribers, value);
            }
        }

        remove
        {
            Delegate.Remove(_dataAvailableSubscribers, value);
        }
    }

    // Process a single sample (mono). For interleaved multi-channel use one instance per channel.
    private float Process(float x, FilterState state)
    {
        float absx = Math.Abs(x);

        // Envelope follower (peak) with separate attack/release coefficients
        if (absx > state.Envelope)
        {
            state.Envelope = attackAlpha * state.Envelope + (1f - attackAlpha) * absx;
        }
        else
        {
            state.Envelope = releaseAlpha * state.Envelope + (1f - releaseAlpha) * absx;
        }

        // If envelope is above threshold, open gate and reset hold counter
        if (state.Envelope >= thresholdLinear)
        {
            state.HoldCounter = holdSamples;
        }

        bool targetOpen = state.HoldCounter > 0;

        if (state.HoldCounter > 0) state.HoldCounter--;

        // Smooth gain towards target (1 if open, 0 if closed) to avoid clicks
        float targetGain = targetOpen ? 1f : 0f;
        if (targetGain > state.Gain)
            state.Gain = attackGainAlpha * state.Gain + (1f - attackGainAlpha) * targetGain;
        else
            state.Gain = releaseGainAlpha * state.Gain + (1f - releaseGainAlpha) * targetGain;

        // Apply gain to sample
        return x * state.Gain;
    }

    protected override void OnDataAvailable(object? sender, WaveInEventArgs e)
    {
        int bytes = e.BytesRecorded;
        int bytesPerSample = AudioFormat.BitsPerSample / 8;
        int channels = AudioFormat.Channels;
        int frameCount = bytes / (bytesPerSample * channels);

        int sampleCount = frameCount * channels;
        if (_dataBuffer == null || _dataBuffer.Length < sampleCount)
            Array.Resize(ref _dataBuffer, sampleCount);

        // Convert raw bytes -> interleaved float samples
        WaveBufferConverter.BytesToFloats(e.Buffer, bytes, AudioFormat, _dataBuffer);

        // Process: per-channel filter (separate state per channel)
        // floatBuffer is interleaved: frame0[ch0], frame0[ch1], frame1[ch0], ...
        for (int frame = 0; frame < frameCount; frame++)
        {
            for (int ch = 0; ch < channels; ch++)
            {
                int idx = frame * channels + ch;
                _dataBuffer[idx] = Process(_dataBuffer[idx], _filterStates[ch]);
            }
        }

        var args = new NoiseGateFilteredWaveInEventArgs(e.Buffer, e.BytesRecorded) { MaxSample = _dataBuffer.Length == 0 ? 0 : _dataBuffer.Max() };
        _dataAvailableSubscribers?.Invoke(this, args);

        base.OnDataAvailable(sender, e);
    }

    private sealed class FilterState
    {
        public float Envelope { get; set; }   // peak envelope follower (0..1)
        public float Gain { get; set; }       // current output gain (0..1)
        public int HoldCounter { get; set; }  // countdown while gate stays open after crossing

        public void Reset(bool startOpen = true)
        {
            Envelope = startOpen ? 0.5f : 0f;
            Gain = startOpen ? 1f : 0f;
            HoldCounter = 0;
        }
    }

    protected override void Dispose(bool disposing)
    {
        _dataAvailableSubscribers = null;

        base.Dispose(disposing);
    }
}
