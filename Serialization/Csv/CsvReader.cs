namespace HM.Serialization.Csv
{
    public sealed class CsvReader : IDisposable, ISheetReader<Cell>
    {
        private sealed class Lock
        {
            public bool IsLocked { get; private set; }

            public void Enter()
            {
                lock (_locker)
                {
                    IsLocked = true;
                }
            }
            public void Exit()
            {
                lock (_locker)
                {
                    IsLocked = false;
                }
            }

            private readonly object _locker = new();
        }

        public string CsvFile { get; private set; } = "";

        public IEnumerable<Cell[]> EnumerateRecords()
        {
            if (locker.IsLocked)
            {
                throw new NotSupportedException();
            }
            try
            {
                locker.Exit();
                ReSeek();
                while (!_reader.EndOfStream)
                {
                    yield return ConvertStringToCells(_reader.ReadLine() ?? "");
                }
            }
            finally
            {
                locker.Exit();
            }
        }
        public IEnumerable<Dictionary<string, Cell>> EnumerateRecordsAsObjects()
        {
            if (locker.IsLocked)
            {
                throw new NotSupportedException();
            }
            try
            {
                locker.Exit();
                ReSeek();
                string[]? headers = (from i in ConvertStringToCells(_reader.ReadLine() ?? "")
                                     select i.ToString())
                               .ToArray();
                while (!_reader.EndOfStream)
                {
                    var dict = new Dictionary<string, Cell>();
                    var cells = ConvertStringToCells(_reader.ReadLine() ?? "");
                    for (int i = 0; i < cells.Length; i++)
                    {
                        dict[headers[i]] = cells[i];
                    }
                    yield return dict;
                }
            }
            finally
            {
                locker.Exit();
            }

        }
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _reader?.Dispose();
            }
        }

        public CsvReader(string csvFile)
        {
            CsvFile = csvFile;
            _reader = new StreamReader(csvFile);
        }

        private readonly StreamReader _reader;
        private readonly Lock locker = new Lock();
        private bool _disposed;
        private static Cell[] ConvertStringToCells(string str)
        {
            return (from i in str.Split(Csv.Delimiter)
                    select new Cell(i))
                    .ToArray();
        }
        private void ReSeek()
        {
            _reader.BaseStream.Seek(0, SeekOrigin.Begin);
        }
    }
}
