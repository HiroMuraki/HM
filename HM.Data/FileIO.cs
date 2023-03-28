namespace HM.Data;

public class FileIO : IFileIO
{
    public static FileIO Instance { get; } = new();

    public bool Exists(string path)
    {
        return File.Exists(path);
    }

    public Stream ReadAsStream(string path)
    {
        return File.OpenRead(path);
    }

    public Stream WriteAsStream(string path)
    {
        string? directory = Path.GetDirectoryName(path);

        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        return File.OpenWrite(path);
    }

    public async Task DeleteFileAsync(string path)
    {
        File.Delete(path);
        await Task.CompletedTask;
    }

    #region NonPublic
    private FileIO()
    {

    }
    #endregion  
}
