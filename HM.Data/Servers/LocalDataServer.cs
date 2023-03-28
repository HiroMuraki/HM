namespace HM.Data.Servers;

public class LocalDataServer : LocalServer, IDataServer
{
    public IDataSerializer DataSerializer { get; }

    public async Task<T> FetchAsync<T>(string serverFilePath, CancellationToken cancellationToken)
    {
        string localFilePath = GetLocalFilePath(serverFilePath);

        using var fs = FileIO.ReadAsStream(localFilePath);
        using var ms = new MemoryStream();

        await WriteToAsync(fs, ms, TranscationMode.Download, cancellationToken);

        return await DataSerializer.DeserializeAsync<T>(ms.ToArray());
    }

    public async Task UploadAsync<T>(T obj, string serverFilePath, CancellationToken cancellationToken)
    {
        string localFilePath = GetLocalFilePath(serverFilePath);

        byte[] data = await DataSerializer.SerializeAsync(obj, cancellationToken);

        using var fs = FileIO.WriteAsStream(localFilePath);

        OnProgressChanged(0, TranscationMode.Upload);

        await fs.WriteAsync(data.AsMemory(), cancellationToken);

        OnProgressChanged(1, TranscationMode.Upload);
    }

    public LocalDataServer(string rootDirectory, IFileIO fileIO, IDataSerializer dataSerializer) : base(rootDirectory, fileIO)
    {
        DataSerializer = dataSerializer;
    }
}

