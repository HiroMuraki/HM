namespace HM.Data.Servers;

public static class IDataServerExtensions
{
    public static async Task<byte[]> FetchAsync(this IDataServer self, string serverFilePath)
    {
        return await self.FetchAsync(serverFilePath, CancellationToken.None);
    }

    public static async Task UploadAsync(this IDataServer self, byte[] data, string serverFilePath)
    {
        await self.UploadAsync(data, serverFilePath, CancellationToken.None);
    }

    public static async Task FreeSpaceAsync(this IDataServer self, string serverFilePath)
    {
        await self.FreeSpaceAsync(serverFilePath, CancellationToken.None);
    }
}

