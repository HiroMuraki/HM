namespace HM.Data.Servers;

public static class IDataServerExtensions
{
    public static async Task<T> FetchAsync<T>(this IDataServer self, string serverFilePath)
    {
        return await self.FetchAsync<T>(serverFilePath, CancellationToken.None);
    }

    public static async Task UploadAsync<T>(this IDataServer self, T data, string serverFilePath)
    {
        await self.UploadAsync(data, serverFilePath, CancellationToken.None);
    }

    public static async Task FreeSpaceAsync(this IDataServer self, string serverFilePath)
    {
        await self.FreeSpaceAsync(serverFilePath, CancellationToken.None);
    }
}

