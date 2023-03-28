namespace HM.Data.Servers;

public class LocalDataServer : LocalServer, IDataServer
{
    public async Task<byte[]> FetchAsync(string serverFilePath, CancellationToken cancellationToken)
    {
        string localFilePath = GetLocalFilePath(serverFilePath);

        using var fs = FileIO.ReadAsStream(localFilePath);
        using var ms = new MemoryStream();

        await WriteToAsync(fs, ms, TranscationMode.Download, cancellationToken);

        return ms.ToArray();
    }

    public async Task UploadAsync(byte[] data, string serverFilePath, CancellationToken cancellationToken)
    {
        string localFilePath = GetLocalFilePath(serverFilePath);

        using var fs = FileIO.WriteAsStream(localFilePath);

        OnProgressChanged(0, TranscationMode.Upload);

        await fs.WriteAsync(data.AsMemory(), cancellationToken);

        OnProgressChanged(1, TranscationMode.Upload);
    }

    public async Task FreeSpaceAsync(string serverFilePath, CancellationToken cancellationToken)
    {
        string localFilePath = GetLocalFilePath(serverFilePath);

        await FileIO.DeleteFileAsync(localFilePath);
    }

    public LocalDataServer(string rootDirectory, IFileIO fileIO) : base(rootDirectory, fileIO)
    {
    }
}

