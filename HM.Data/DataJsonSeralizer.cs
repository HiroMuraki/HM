using System.Text.Encodings.Web;
using System.Text.Json;

namespace HM.Data;

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

