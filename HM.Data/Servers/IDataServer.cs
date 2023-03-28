namespace HM.Data.Servers;

public interface IDataServer
{
    event EventHandler<ProgressChangedEventArgs>? ProgressChanged;
    event EventHandler<ExceptionRaisedEventArgs>? ExceptionRaised;

    Task<byte[]> FetchAsync(string serverFilePath, CancellationToken cancellationToken);

    Task UploadAsync(byte[] data, string serverFilePath, CancellationToken cancellationToken);

    Task FreeSpaceAsync(string serverFilePath, CancellationToken cancellationToken);
}

