namespace HM.Data.Servers;

public static class IFileServerExtensions
{
    public static async Task<bool> CheckIfFileExisted(this IFileServer server, string serverFilePath)
    {
        return await server.CheckIfFileExisted(serverFilePath, CancellationToken.None);
    }

    public static async Task UploadAsync(this IFileServer server, string serverFilePath, string localFilePath)
    {
        await server.UploadAsync(serverFilePath, localFilePath, CancellationToken.None);
    }

    public static async Task DownloadAsync(this IFileServer server, string serverFilePath, string localFilePath)
    {
        await server.DownloadAsync(serverFilePath, localFilePath, CancellationToken.None);
    }

    public static async Task RemoveAsync(this IFileServer server, string serverFilePath)
    {
        await server.RemoveAsync(serverFilePath, CancellationToken.None);
    }
}
