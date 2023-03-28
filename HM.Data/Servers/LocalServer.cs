namespace HM.Data.Servers;

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