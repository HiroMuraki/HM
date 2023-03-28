namespace HM.Data.Servers;

public class ProgressChangedEventArgs : EventArgs
{
    public TranscationMode TranscationMode { get; init; }
    public double Progress { get; init; }
    public bool TaskCompleted { get; init; }
}
