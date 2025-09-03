namespace Later.App;

internal interface IAudioCaptureDevice<TAudioFormat, TAudioData, TRecordingStoppedEventArgs> : IAudioSampleProvider<TAudioData>, IDisposable
{
    event EventHandler<TRecordingStoppedEventArgs> CaptureStopped;
    Task StartCapturingAsync();
    Task StopCapturingAsync();
    CaptureState CaptureState { get; }
    TAudioFormat AudioFormat { get; }

    void IDisposable.Dispose()
    {
    }
}

internal interface IAudioSampleProvider<TAudioData>
{
    event EventHandler<TAudioData> DataAvailable;
}
