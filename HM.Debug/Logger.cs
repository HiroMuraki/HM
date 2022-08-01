using System.Text;

namespace HM.Debug
{
    [Flags]
    public enum LogLevel
    {
        #pragma warning disable format
        None        = 0b0000_0000,
        Any         = 0b0000_0001,
        Information = 0b0000_0010,
        Warning     = 0b0000_0100,
        Error       = 0b0000_1000,
        Fatal       = 0b0001_0000,
        #pragma warning restore format
    }
    public sealed class Logger : IDisposable
    {
        public string File { get; init; }
        public LogLevel Level
        {
            get => _level;
            set
            {
                _level = value;
                _levelValue = (int)value;
            }
        }
        public long MaxFileSize
        {
            get => _maxFileSize;
            private set
            {
                if (value < 0)
                {
                    value = 0;
                }
                _maxFileSize = value;
            }
        }

        public void WriteLine(string message, LogLevel logLevel)
        {
            WriteLineCore(message, logLevel);
        }
        public void SetMaxFileSize(int mbSize, int kbSize, int byteSize)
        {
            MaxFileSize = byteSize
                          + kbSize * _fileSizeBase
                          + mbSize * _fileSizeBase * _fileSizeBase;
        }
        public void Dispose()
        {
            _writer.Close();
            _writer.Dispose();
        }

        public Logger(string logFile) : this(logFile, LogLevel.Warning) { }
        public Logger(string logFile, LogLevel level)
        {
            File = logFile;
            Level = level;
            _writer = new StreamWriter(logFile, true, Encoding.UTF8);
        }

        private readonly StreamWriter _writer;
        private const int _fileSizeBase = 1024;
        private long _maxFileSize = 0;
        private int _levelValue;
        private LogLevel _level = LogLevel.Warning;
        private void WriteLineCore(string message, LogLevel logLevel)
        {
            if (logLevel == LogLevel.None)
            {
                return;
            }
            var l = logLevel & Level;
            if (l == _level || (l == logLevel && (int)logLevel > _levelValue))
            {
                if (_writer.BaseStream.Length >= MaxFileSize)
                {
                    throw new IOException($"max file size `{_maxFileSize}` reached");
                }
                _writer.WriteLine(message);
            }
        }
    }
}
