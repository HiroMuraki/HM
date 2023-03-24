using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System;
using System.Threading;
using HM.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace HM.Data;

public interface ISingleData<T>
{
    T? Get();

    int Update(T item);

    int Delete();
}

public class ProgressChangedEventArgs : EventArgs
{
    public double Progress { get; init; }
    public bool TaskCompleted { get; init; }
}

public class ExceptionRaisedEventArgs : EventArgs
{
    public Exception? Exception { get; init; }
}

public interface IFileServer
{
    /// <summary>
    /// Raised when the current uploading progress has changed.
    /// </summary>
    event EventHandler<ProgressChangedEventArgs>? UploadingProgressChanged;

    /// <summary>
    /// Raised when the current downloading progress has changed.
    /// </summary>
    event EventHandler<ProgressChangedEventArgs>? DownloadingProgressChanged;

    /// <summary>
    /// Raised when an exception is raised during a file transfer operation.
    /// </summary>
    event EventHandler<ExceptionRaisedEventArgs>? ExceptionRaised;

    /// <summary>
    /// Checks if a file exists on the server.
    /// </summary>
    /// <param name="serverFilePath">The path to the file on the server.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>A Task representing the asynchronous operation. The task result contains a boolean value indicating whether the file exists (true) or not (false).</returns>
    Task<bool> CheckIfFileExisted(string serverFilePath, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously uploads a file to the server.
    /// </summary>
    /// <param name="serverFilePath">The destination path on the server where the file will be uploaded.</param>
    /// <param name="localFilePath">The local path of the file to upload.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    Task UploadAsync(string serverFilePath, string localFilePath, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously downloads a file from the server.
    /// </summary>
    /// <param name="serverFilePath">The path of the file to download from the server.</param>
    /// <param name="localFilePath">The local path where the downloaded file will be saved.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    Task DownloadAsync(string serverFilePath, string localFilePath, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously removes a file from the server at the specified path.
    /// </summary>
    /// <param name="serverFilePath">The path of the file to remove on the server.</param>
    /// <param name="cancellationToken">The cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation of removing the file.</returns>
    Task RemoveAsync(string serverFilePath, CancellationToken cancellationToken);
}

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
        await Task.Run(() =>
        {
            File.Delete(path);
        });
    }

    #region NonPublic
    private FileIO()
    {

    }
    #endregion  
}

public class LocalFileServer : IFileServer
{
    public const int BufferSize = 16 * 1024; // 16KB buffer size
    public const int SizeToEnableBuffer = 1 * 1024 * 1024; // Read/Write file larger than 1MB will enable buffer

    public event EventHandler<ProgressChangedEventArgs>? UploadingProgressChanged;
    public event EventHandler<ProgressChangedEventArgs>? DownloadingProgressChanged;
    public event EventHandler<ExceptionRaisedEventArgs>? ExceptionRaised;

    public Task<bool> CheckIfFileExisted(string serverFilePath, CancellationToken cancellationToken)
    {
        string targetFilePath = Path.Combine(_rootDirectory, serverFilePath);

        return Task.FromResult(_fileIO.Exists(targetFilePath));
    }

    public async Task DownloadAsync(string serverFilePath, string localFilePath, CancellationToken cancellationToken)
    {
        string sourceLocalFilePath = Path.Combine(_rootDirectory, serverFilePath);

        if (!_fileIO.Exists(sourceLocalFilePath))
        {
            ExceptionRaised?.Invoke(this, new ExceptionRaisedEventArgs()
            {
                Exception = new FileNotFoundException($"Can't find file `{sourceLocalFilePath}`")
            });
            return;
        }

        using var sourceFileStream = _fileIO.ReadAsStream(sourceLocalFilePath);
        using var targetFileStream = _fileIO.WriteAsStream(localFilePath);

        await WriteToAsync(
            sourceFileStream,
            targetFileStream,
            cancellationToken,
            p => DownloadingProgressChanged?.Invoke(this, p)
        );
    }

    public async Task UploadAsync(string serverFilePath, string localFilePath, CancellationToken cancellationToken)
    {
        string targetFilePath = Path.Combine(_rootDirectory, serverFilePath);

        using var targetFileStream = _fileIO.WriteAsStream(targetFilePath);
        using var sourceFileStream = _fileIO.ReadAsStream(localFilePath);

        await WriteToAsync(
            sourceFileStream,
            targetFileStream,
            cancellationToken,
            p => UploadingProgressChanged?.Invoke(this, p)
        );
    }

    public async Task RemoveAsync(string serverFilePath, CancellationToken cancellationToken)
    {
        try
        {
            string targetFilePath = Path.Combine(_rootDirectory, serverFilePath);

            await _fileIO.DeleteFileAsync(targetFilePath);
        }
        catch (Exception e)
        {
            ExceptionRaised?.Invoke(this, new ExceptionRaisedEventArgs()
            {
                Exception = e
            });
        }
    }

    public LocalFileServer(string rootDirectory, IFileIO fileIO)
    {
        _rootDirectory = rootDirectory;
        _fileIO = fileIO;
    }

    #region NonPublic
    private readonly string _rootDirectory;
    private readonly IFileIO _fileIO;
    private async Task WriteToAsync(Stream sourceFileStream, Stream targetFileStream, CancellationToken cancellationToken, Action<ProgressChangedEventArgs> progressChanged)
    {
        try
        {
            progressChanged?.Invoke(new ProgressChangedEventArgs()
            {
                Progress = 0,
                TaskCompleted = false
            });

            if (sourceFileStream.Length <= SizeToEnableBuffer)
            {
                await sourceFileStream.CopyToAsync(targetFileStream, cancellationToken);
            }
            else
            {
                int readCount;
                byte[] buffer = new byte[BufferSize];
                while ((readCount = sourceFileStream.Read(buffer)) > 0)
                {
                    await targetFileStream.WriteAsync(buffer.AsMemory(0, readCount), cancellationToken);

                    progressChanged?.Invoke(new ProgressChangedEventArgs()
                    {
                        Progress = (double)targetFileStream.Length / sourceFileStream.Length,
                        TaskCompleted = false
                    });
                }

            }

            progressChanged?.Invoke(new ProgressChangedEventArgs()
            {
                Progress = 1,
                TaskCompleted = true
            });
        }
        catch (TaskCanceledException e)
        {
            ExceptionRaised?.Invoke(this, new ExceptionRaisedEventArgs()
            {
                Exception = e
            });
        }
        catch (Exception e)
        {
            ExceptionRaised?.Invoke(this, new ExceptionRaisedEventArgs()
            {
                Exception = e
            });
        }
    }
    #endregion
}