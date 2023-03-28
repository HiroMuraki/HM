namespace HM.Data;

/// <summary>
/// Provides methods for interacting with files on disk.
/// </summary>
public interface IFileIO
{
    /// <summary>
    /// Determines whether the specified file exists.
    /// </summary>
    /// <param name="path">The path to the file to check.</param>
    /// <returns>true if the file exists; otherwise, false.</returns>
    bool Exists(string path);

    /// <summary>
    /// Opens a read-only stream for the specified file.
    /// </summary>
    /// <param name="path">The path to the file to open.</param>
    /// <returns>A read-only stream for the specified file.</returns>
    Stream ReadAsStream(string path);

    /// <summary>
    /// Opens a write-only stream for the specified file. If the file does not exist, it is created. If the directory containing the file does not exist, it is also created.
    /// </summary>
    /// <param name="path">The path to the file to open.</param>
    /// <returns>A write-only stream for the specified file.</returns>
    Stream WriteAsStream(string path);

    /// <summary>
    /// Asynchronously deletes the specified file.
    /// </summary>
    /// <param name="path">The path to the file to delete.</param>
    /// <returns>A task representing the asynchronous operation of deleting the file.</returns>
    Task DeleteFileAsync(string path);
}
