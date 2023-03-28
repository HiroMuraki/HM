namespace HM.Data.Servers;

/// <summary>
/// Server of files
/// </summary>
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
