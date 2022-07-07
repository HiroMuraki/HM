using System.Collections;
using System.Collections.Immutable;

namespace HM.Serialization.Csv
{
    public sealed class CsvSheet : IEnumerable<Cell[]>, IEnumerable
    {
        public Cell this[int row, int column]
        {
            get
            {
                return _records[row][column];
            }
        }

        public static CsvSheet Load(ISheetReader<Cell> textReader)
        {
            return new CsvSheet((from row in textReader.EnumerateRecords()
                                 select row.ToList()).ToList());
        }
        public static CsvSheet Load(string csvFile)
        {
            return Load(Csv.Read(csvFile));
        }
        public IEnumerable<Cell[]> EnumerateRecords()
        {
            foreach (var item in this)
            {
                yield return item;
            }
        }
        public IEnumerable<Cell[]> FindRecords(Predicate<Cell[]> predicate)
        {
            foreach (var item in _records)
            {
                var row = item.ToArray();
                if (predicate(row))
                {
                    yield return row;
                }
            }
        }
        public IEnumerable<Cell[]> FindRecords(Predicate<Dictionary<string, Cell>> predicate)
        {
            foreach (var item in EnumerateRecordsAsObjects())
            {
                if (predicate(item))
                {
                    yield return item.Values.ToArray();
                }
            }
        }
        public IEnumerable<Dictionary<string, Cell>> FindRecordsAsObjects(Predicate<Dictionary<string, Cell>> predicate)
        {
            string[]? headers = (from i in _records[0]
                                 select i.ToString()).ToArray();

            foreach (var item in _records.Skip(1))
            {
                var t = new Dictionary<string, Cell>();
                for (int i = 0; i < headers.Length; i++)
                {
                    t[headers[i]] = item[i];
                }
                if (predicate(t))
                {
                    yield return t;
                }
            }
        }
        public IEnumerable<Dictionary<string, Cell>> EnumerateRecordsAsObjects()
        {
            return FindRecordsAsObjects(c => true);
        }
        public IEnumerable<Cell> GetRow(int row)
        {
            return _records[row];
        }
        public IEnumerable<Cell> GetColumn(int column)
        {
            for (int i = 0; i < _records.Length; i++)
            {
                yield return _records[i][column];
            }
        }
        public IEnumerable<Cell> GetColumn(string header)
        {
            return GetColumn((from i in _records[0]
                              select i.ToString())
                              .ToList()
                              .IndexOf(header));
        }
        public IEnumerator<Cell[]> GetEnumerator()
        {
            foreach (var item in _records)
            {
                yield return item.ToArray();
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private readonly ImmutableArray<ImmutableArray<Cell>> _records;

        private CsvSheet(List<List<Cell>> data)
        {
            var lst = new ImmutableArray<Cell>[data.Count];
            for (int row = 0; row < data.Count; row++)
            {
                var t = new List<Cell>();
                for (int col = 0; col < data[row].Count; col++)
                {
                    t.Add(data[row][col]);
                }
                lst[row] = ImmutableArray.CreateRange(t);
            }
            _records = ImmutableArray.CreateRange(lst);
        }
    }
}