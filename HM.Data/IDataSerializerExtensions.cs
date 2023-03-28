namespace HM.Data;

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

