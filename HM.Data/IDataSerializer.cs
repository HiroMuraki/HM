namespace HM.Data;

public interface IDataSerializer
{
    Task<byte[]> SerializeAsync<T>(T obj, CancellationToken cancellationToken);

    Task<T> DeserializeAsync<T>(byte[] data, CancellationToken cancellationToken);
}

