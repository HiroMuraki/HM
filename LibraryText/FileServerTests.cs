using HM.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime;

namespace LibraryTest;

[TestClass]
public class FileServerTests
{
    private static readonly string _localRoot = @"C:\Users\11717\Downloads\ServerTest\LocalData";
    private static readonly string _serverRoot = @"C:\Users\11717\Downloads\ServerTest\ServerData";
    private static readonly string _tempFile = @"testFile.txt";
    private static readonly string _serverFilePath = @"serverFile.txt";
    private static string TempFileFullPath => Path.Combine(_localRoot, _tempFile);
    private static string ServerFileFullPath => Path.Combine(_serverRoot, _serverFilePath);
    private static readonly int _fileSizeBytes = 32 * 1024 * 1024; // 32MB in bytes
    private readonly IFileServer _server = new LocalFileServer(_serverRoot, FileIO.Instance);

    [TestMethod]
    public async Task UploadAsync_UploadsFileToServer()
    {
        var cancel = new CancellationTokenSource();
        var cancellationToken = cancel.Token;

        // Cleanup
        if (File.Exists(TempFileFullPath))
        {
            File.Delete(TempFileFullPath);
        }
        if (await _server.CheckIfFileExisted(_serverFilePath, cancellationToken))
        {
            await _server.RemoveAsync(_serverFilePath, cancellationToken);
        }

        if (!Directory.Exists(_localRoot))
        {
            Directory.CreateDirectory(_localRoot);
        }
        // Create a temporary file to upload
        using (var stream = new FileStream(TempFileFullPath, FileMode.Create))
        {
            stream.Seek(_fileSizeBytes - 1, SeekOrigin.Begin);
            stream.WriteByte(0);
        }
        System.Diagnostics.Debug.WriteLine($"Local file created"); // debug output

        // Add a progress changed listener
        double uploadProgress = -1;
        double lastLoggedUploadProgress = -1;
        _server.ProgressChanged += (sender, args) =>
        {
            uploadProgress = args.Progress;

            if (uploadProgress - lastLoggedUploadProgress >= 0.1)
            {
                lastLoggedUploadProgress = uploadProgress;
                System.Diagnostics.Debug.WriteLine($"[Uploading] {uploadProgress * 100:F2}%"); // debug output
            }

            if (args.TaskCompleted)
            {
                System.Diagnostics.Debug.WriteLine($"[{File.Exists(ServerFileFullPath)}] Upload to server completed"); // debug output
            }
        };

        // Act
        await _server.UploadAsync(_serverFilePath, TempFileFullPath, cancellationToken);

        // Assert
        bool fileExists = await _server.CheckIfFileExisted(_serverFilePath, cancellationToken);
        Assert.IsTrue(fileExists);

        // Verify that the progress changed event was raised at least once
        Assert.IsTrue(uploadProgress > 0);

        // Cleanup
        Directory.Delete(_localRoot, true);
        Directory.Delete(_serverRoot, true);
    }

    [TestMethod]
    public async Task UploadErrorAsync_UploadsFileToServer()
    {
        var cancel = new CancellationTokenSource();
        var cancellationToken = cancel.Token;

        // Cleanup
        if (File.Exists(TempFileFullPath))
        {
            File.Delete(TempFileFullPath);
        }
        if (await _server.CheckIfFileExisted(_serverFilePath, cancellationToken))
        {
            await _server.RemoveAsync(_serverFilePath, cancellationToken);
        }

        if (!Directory.Exists(_localRoot))
        {
            Directory.CreateDirectory(_localRoot);
        }
        // Create a temporary file to upload
        using (var stream = new FileStream(TempFileFullPath, FileMode.Create))
        {
            stream.Seek(_fileSizeBytes - 1, SeekOrigin.Begin);
            stream.WriteByte(0);
        }
        System.Diagnostics.Debug.WriteLine($"Local file created"); // debug output

        // Add a progress changed listener
        double uploadProgress = -1;
        _server.ProgressChanged += (sender, args) =>
        {
            uploadProgress = args.Progress;
            System.Diagnostics.Debug.WriteLine($"[Uploading] {uploadProgress * 100:F2}%"); // debug output

            if (uploadProgress >= 0.01)
            {
                cancel.Cancel();
            }
        };

        // Add a exception raised listener
        _server.ExceptionRaised += (s, e) =>
        {
            System.Diagnostics.Debug.WriteLine($"[Error] {e.Exception?.Message}"); // debug output
        };

        // Act
        await _server.UploadAsync(_serverFilePath, TempFileFullPath, cancellationToken);

        // Assert
        bool fileExists = await _server.CheckIfFileExisted(_serverFilePath, cancellationToken);
        Assert.IsTrue(fileExists);

        // Verify that the progress changed event was raised at least once
        Assert.IsTrue(uploadProgress > 0);

        // Cleanup
        Directory.Delete(_localRoot, true);
        Directory.Delete(_serverRoot, true);
    }

    [TestMethod]
    public async Task DownloadAsync_DownloadsFileFromServer()
    {
        var cancel = new CancellationTokenSource();
        var cancellationToken = cancel.Token;

        // Cleanup
        if (File.Exists(TempFileFullPath))
        {
            File.Delete(TempFileFullPath);
        }
        if (!await _server.CheckIfFileExisted(_serverFilePath, cancellationToken))
        {
            if (!Directory.Exists(_serverRoot))
            {
                Directory.CreateDirectory(_serverRoot);
            }
            using (var stream = new FileStream(ServerFileFullPath, FileMode.Create))
            {
                stream.Seek(_fileSizeBytes - 1, SeekOrigin.Begin);
                stream.WriteByte(0);
                System.Diagnostics.Debug.WriteLine($"Server file Created"); // debug output
            }
        }

        // Add a progress changed listener
        double downloadProgress = -1;
        double lastLoggedDownloadProgress = -1;
        _server.ProgressChanged += (sender, args) =>
        {
            downloadProgress = args.Progress;

            if (downloadProgress - lastLoggedDownloadProgress >= 0.1)
            {
                lastLoggedDownloadProgress = downloadProgress;
                System.Diagnostics.Debug.WriteLine($"[Downloading] {downloadProgress * 100:F2}%"); // debug output
            }

            if (args.TaskCompleted)
            {
                System.Diagnostics.Debug.WriteLine($"[{File.Exists(ServerFileFullPath)}] Download from server completed"); // debug output
            }
        };

        // Act
        await _server.DownloadAsync(_serverFilePath, TempFileFullPath, cancellationToken);

        // Assert
        Assert.IsTrue(File.Exists(TempFileFullPath));

        // Verify that the progress changed event was raised at least once
        Assert.IsTrue(downloadProgress > 0);

        // Cleanup
        Directory.Delete(_localRoot, true);
        Directory.Delete(_serverRoot, true);
    }

    [TestMethod]
    public async Task DownloadErrorAsync_DownloadsFileFromServer()
    {
        var cancel = new CancellationTokenSource();
        var cancellationToken = cancel.Token;

        // Cleanup
        if (File.Exists(TempFileFullPath))
        {
            File.Delete(TempFileFullPath);
        }
        if (!await _server.CheckIfFileExisted(_serverFilePath, cancellationToken))
        {
            if (!Directory.Exists(_serverRoot))
            {
                Directory.CreateDirectory(_serverRoot);
            }
            using (var stream = new FileStream(ServerFileFullPath, FileMode.Create))
            {
                stream.Seek(_fileSizeBytes - 1, SeekOrigin.Begin);
                stream.WriteByte(0);
                System.Diagnostics.Debug.WriteLine($"Server file Created"); // debug output
            }
        }

        // Add a progress changed listener
        double downloadProgress = -1;
        _server.ProgressChanged += (sender, args) =>
        {
            downloadProgress = args.Progress;
            System.Diagnostics.Debug.WriteLine($"[Downloading] {downloadProgress * 100:F2}%"); // debug output
            if (downloadProgress >= 0.01)
            {
                cancel.Cancel();
            }
        };

        // Add a exception raised listener
        _server.ExceptionRaised += (s, e) =>
        {
            System.Diagnostics.Debug.WriteLine($"[Error] {e.Exception?.Message}"); // debug output
        };

        // Act
        await _server.DownloadAsync(_serverFilePath, TempFileFullPath, cancellationToken);

        // Assert
        Assert.IsTrue(File.Exists(TempFileFullPath));

        // Verify that the progress changed event was raised at least once
        Assert.IsTrue(downloadProgress > 0);

        // Cleanup
        Directory.Delete(_localRoot, true);
        Directory.Delete(_serverRoot, true);
    }

}
