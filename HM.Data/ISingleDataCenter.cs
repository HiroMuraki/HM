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
using System.Net.Mail;
using System.Xml;

namespace HM.Data;

public interface ISingleDataCenter<T>
{
    T? Get();

    int Update(T item);

    int Delete();
}

public enum TranscationMode
{
    Undefined,
    Upload,
    Download
}

public class ProgressChangedEventArgs : EventArgs
{
    public TranscationMode TranscationMode { get; init; }
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
    /// Raised when the current uploading or downloading progress has changed.
    /// </summary>
    event EventHandler<ProgressChangedEventArgs>? ProgressChanged;

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
        File.Delete(path);
        await Task.CompletedTask;
    }

    #region NonPublic
    private FileIO()
    {

    }
    #endregion  
}

public class LocalFileServer : LocalServer, IFileServer
{
    public Task<bool> CheckIfFileExisted(string serverFilePath, CancellationToken cancellationToken)
    {
        string targetFilePath = GetLocalFilePath(serverFilePath);

        return Task.FromResult(FileIO.Exists(targetFilePath));
    }

    public async Task DownloadAsync(string serverFilePath, string localFilePath, CancellationToken cancellationToken)
    {
        string sourceLocalFilePath = GetLocalFilePath(serverFilePath);

        if (!FileIO.Exists(sourceLocalFilePath))
        {
            OnExceptionRaised(new FileNotFoundException($"Can't find file `{sourceLocalFilePath}`"));
            return;
        }

        using var sourceFileStream = FileIO.ReadAsStream(sourceLocalFilePath);
        using var targetFileStream = FileIO.WriteAsStream(localFilePath);

        await WriteToAsync(
            sourceFileStream,
            targetFileStream,
            TranscationMode.Download,
            cancellationToken
        );
    }

    public async Task UploadAsync(string serverFilePath, string localFilePath, CancellationToken cancellationToken)
    {
        string targetFilePath = GetLocalFilePath(serverFilePath);

        using var targetFileStream = FileIO.WriteAsStream(targetFilePath);
        using var sourceFileStream = FileIO.ReadAsStream(localFilePath);

        await WriteToAsync(
            sourceFileStream,
            targetFileStream,
            TranscationMode.Upload,
            cancellationToken
        );
    }

    public async Task RemoveAsync(string serverFilePath, CancellationToken cancellationToken)
    {
        try
        {
            string targetFilePath = GetLocalFilePath(serverFilePath);

            await FileIO.DeleteFileAsync(targetFilePath);
        }
        catch (Exception e)
        {
            OnExceptionRaised(e);
        }
    }

    public LocalFileServer(string rootDirectory, IFileIO fileIO) : base(rootDirectory, fileIO)
    {
    }
}

public abstract class LocalServer
{
    public const int BufferSize = 16 * 1024; // 16KB buffer size
    public const int SizeToEnableBuffer = 1 * 1024 * 1024; // Read/Write file larger than 1MB will enable buffer

    public event EventHandler<ProgressChangedEventArgs>? ProgressChanged;
    public event EventHandler<ExceptionRaisedEventArgs>? ExceptionRaised;

    public LocalServer(string rootDirectory, IFileIO fileIO)
    {
        _rootDirectory = rootDirectory;
        _fileIO = fileIO;
    }

    #region NonPublic
    protected string RootDirectoy => _rootDirectory;
    protected IFileIO FileIO => _fileIO;
    protected string GetLocalFilePath(string serverFilePath)
    {
        return Path.Combine(_rootDirectory, serverFilePath);
    }
    protected void OnProgressChanged(double progress, TranscationMode transcationMode)
    {
        ProgressChanged?.Invoke(this, new ProgressChangedEventArgs()
        {
            TranscationMode = transcationMode,
            Progress = progress,
            TaskCompleted = progress == 1
        });
    }
    protected void OnExceptionRaised(Exception exception)
    {
        ExceptionRaised?.Invoke(this, new ExceptionRaisedEventArgs()
        {
            Exception = exception
        });
    }
    protected async Task WriteToAsync(Stream sourceStream, Stream targetStream, TranscationMode transcationMode, CancellationToken cancellationToken)
    {
        try
        {
            OnProgressChanged(0, transcationMode);

            if (sourceStream.Length <= SizeToEnableBuffer)
            {
                await sourceStream.CopyToAsync(targetStream, cancellationToken);
            }
            else
            {
                int readCount;
                byte[] buffer = new byte[BufferSize];
                while ((readCount = sourceStream.Read(buffer)) > 0)
                {
                    await targetStream.WriteAsync(buffer.AsMemory(0, readCount), cancellationToken);

                    OnProgressChanged((double)targetStream.Length / sourceStream.Length, transcationMode);
                }
            }

            OnProgressChanged(1, transcationMode);
        }
        catch (TaskCanceledException e)
        {
            OnExceptionRaised(e);
        }
        catch (Exception e)
        {
            OnExceptionRaised(e);
        }
    }

    private readonly string _rootDirectory;
    private readonly IFileIO _fileIO;
    #endregion
}