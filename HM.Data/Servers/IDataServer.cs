namespace HM.Data.Servers;

public interface IDataServer
{
    event EventHandler<ProgressChangedEventArgs>? ProgressChanged;
    event EventHandler<ExceptionRaisedEventArgs>? ExceptionRaised;

    Task<T> FetchAsync<T>(string serverFilePath, CancellationToken cancellationToken);

    Task UploadAsync<T>(T data, string serverFilePath, CancellationToken cancellationToken);
}

