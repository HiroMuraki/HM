namespace HM.Data.Servers;

public class ExceptionRaisedEventArgs : EventArgs
{
    public Exception? Exception { get; init; }
}
