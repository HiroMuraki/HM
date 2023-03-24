[TestClass]
public class ServerTests
{
    private IServer _server;

    [TestInitialize]
    public void Setup()
    {
        // Initialize the server instance
        _server = new LocalServer();
    }

    [TestMethod]
    public async Task UploadAsync_Successful()
    {
        // Arrange
        string serverFilePath = "/path/to/server/file.txt";
        string localFilePath = "C:\\path\\to\\local\\file.txt";
        CancellationToken cancellationToken = CancellationToken.None;

        // Act
        await _server.UploadAsync(serverFilePath, localFilePath, cancellationToken);

        // Assert
        // Check if the file was uploaded successfully to the server
        Assert.IsTrue(_server.FileExists(serverFilePath));
    }

    [TestMethod]
    public async Task DownloadAsync_Successful()
    {
        // Arrange
        string serverFilePath = "/path/to/server/file.txt";
        string localFilePath = "C:\\path\\to\\local\\file.txt";
        CancellationToken cancellationToken = CancellationToken.None;

        // Act
        await _server.DownloadAsync(serverFilePath, localFilePath, cancellationToken);

        // Assert
        // Check if the file was downloaded successfully to the local machine
        Assert.IsTrue(File.Exists(localFilePath));
    }

    [TestMethod]
    public async Task UploadAsync_ExceptionRaised()
    {
        // Arrange
        string serverFilePath = "/path/to/server/file.txt";
        string localFilePath = "C:\\path\\to\\local\\file.txt";
        CancellationToken cancellationToken = CancellationToken.None;

        // Simulate an exception being raised during the upload operation
        _server.ExceptionRaised += (sender, args) =>
        {
            throw args.Exception;
        };

        // Act and Assert
        // Check if an exception is raised when uploading a non-existent file to the server
        await Assert.ThrowsExceptionAsync<FileNotFoundException>(() => _server.UploadAsync(serverFilePath, "C:\\path\\to\\nonexistent\\file.txt", cancellationToken));
    }

    [TestMethod]
    public async Task DownloadAsync_ExceptionRaised()
    {
        // Arrange
        string serverFilePath = "/path/to/server/file.txt";
        string localFilePath = "C:\\path\\to\\local\\file.txt";
        CancellationToken cancellationToken = CancellationToken.None;

        // Simulate an exception being raised during the download operation
        _server.ExceptionRaised += (sender, args) =>
        {
            throw args.Exception;
        };

        // Act and Assert
        // Check if an exception is raised when downloading a non-existent file from the server
        await Assert.ThrowsExceptionAsync<FileNotFoundException>(() => _server.DownloadAsync("/path/to/nonexistent/file.txt", localFilePath, cancellationToken));
    }
}
