namespace HM.Data.Servers;

public class LocalFileServer : LocalServer, IFileServer
{
    public Task<bool> CheckIfFileExisted(string serverFilePath, CancellationToken cancellationToken)
    {
        string targetFilePath = GetLocalFilePath(serverFilePath);

        return Task.FromResult(FileIO.Exists(targetFilePath));
    }

    public async Task DownloadAsync(string serverFilePath, string localFilePath, CancellationToken cancellationToken)
    {
        string sourceLocalFilePath = GetLocalFilePath(serverFilePath);

        if (!FileIO.Exists(sourceLocalFilePath))
        {
            OnExceptionRaised(new FileNotFoundException($"Can't find file `{sourceLocalFilePath}`"));
            return;
        }

        using var sourceFileStream = FileIO.ReadAsStream(sourceLocalFilePath);
        using var targetFileStream = FileIO.WriteAsStream(localFilePath);

        await WriteToAsync(
            sourceFileStream,
            targetFileStream,
            TranscationMode.Download,
            cancellationToken
        );
    }

    public async Task UploadAsync(string serverFilePath, string localFilePath, CancellationToken cancellationToken)
    {
        string targetFilePath = GetLocalFilePath(serverFilePath);

        using var targetFileStream = FileIO.WriteAsStream(targetFilePath);
        using var sourceFileStream = FileIO.ReadAsStream(localFilePath);

        await WriteToAsync(
            sourceFileStream,
            targetFileStream,
            TranscationMode.Upload,
            cancellationToken
        );
    }

    public async Task RemoveAsync(string serverFilePath, CancellationToken cancellationToken)
    {
        try
        {
            string targetFilePath = GetLocalFilePath(serverFilePath);

            await FileIO.DeleteFileAsync(targetFilePath);
        }
        catch (Exception e)
        {
            OnExceptionRaised(e);
        }
    }

    public LocalFileServer(string rootDirectory, IFileIO fileIO) : base(rootDirectory, fileIO)
    {
    }
}
