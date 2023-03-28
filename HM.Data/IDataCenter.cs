using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace HM.Data;

public interface IDataCenter<T>
{
    T? Get(Predicate<T> predicate);

    T[] GetMany(Predicate<T> predicate);

    int Add(T item);

    int AddMany(T[] items);

    int Update(Predicate<T> predicate, T newValue);

    int UpdateMany(Predicate<T> predicate, T newValue);

    int Delete(Predicate<T> predicate);

    int DeleteMany(Predicate<T> predicate);
}

public interface IDataServer
{
    event EventHandler<ProgressChangedEventArgs>? ProgressChanged;
    event EventHandler<ExceptionRaisedEventArgs>? ExceptionRaised;

    Task<T> FetchAsync<T>(string serverFilePath, CancellationToken cancellationToken);

    Task UploadAsync<T>(T data, string serverFilePath, CancellationToken cancellationToken);
}

public static class IDataServerExtensions
{
    public static async Task<T> FetchAsync<T>(this IDataServer self, string serverFilePath)
    {
        return await self.FetchAsync<T>(serverFilePath, CancellationToken.None);
    }

    public static async Task UploadAsync<T>(this IDataServer self, T data, string serverFilePath)
    {
        await self.UploadAsync<T>(data, serverFilePath, CancellationToken.None);
    }
}

public interface IDataSerializer
{
    Task<byte[]> SerializeAsync<T>(T obj, CancellationToken cancellationToken);

    Task<T> DeserializeAsync<T>(byte[] data, CancellationToken cancellationToken);
}

public static class IDataSerializerExtensions
{
    public static async Task<byte[]> SerializeAsync<T>(this IDataSerializer serlf, T obj)
    {
        return await serlf.SerializeAsync<T>(obj, CancellationToken.None);
    }

    public static async Task<T> DeserializeAsync<T>(this IDataSerializer self, byte[] data)
    {
        return await self.DeserializeAsync<T>(data, CancellationToken.None);
    }
}

public class DataJsonSeralizer : IDataSerializer
{
    public static DataJsonSeralizer Instance { get; } = new();

    public async Task<T> DeserializeAsync<T>(byte[] data, CancellationToken cancellationToken)
    {
        var result = JsonSerializer.Deserialize<T>(data, _serializerOptions)
            ?? throw new InvalidOperationException("Unable to deserialize data");

        return await Task.FromResult(result);
    }

    public async Task<byte[]> SerializeAsync<T>(T obj, CancellationToken cancellationToken)
    {
        byte[] data = JsonSerializer.SerializeToUtf8Bytes<T>(obj, _serializerOptions);

        return await Task.FromResult(data);
    }


    #region NonPublic
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true,
    };
    #endregion
}

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

