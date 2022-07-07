namespace HM.Serialization.Csv
{
    public sealed class CsvWriter
    {
        public string CsvFile { get; private set; }

        public CsvWriter(string csvFile)
        {
            CsvFile = csvFile;
        }
        public void AppendRecord(params Cell[] record)
        {
            _recordsCache.Add(record);
        }
        public void AppendRecord(IEnumerable<Cell> record)
        {
            _recordsCache.Add(record);
        }
        public void InsertRecord(int index, IEnumerable<Cell> record)
        {
            _recordsCache.Insert(index, record);
        }
        public void RemoveRecord(int index)
        {
            _recordsCache.RemoveAt(index);
        }
        public void Flush()
        {
            using (var writer = new StreamWriter(CsvFile))
            {
                for (int i = 0; i < _recordsCache.Count; i++)
                {
                    writer.Write(string.Join(Csv.Delimiter, _recordsCache[i]));
                    if (i != _recordsCache.Count - 1)
                    {
                        writer.WriteLine();
                    }
                }
            }
        }
        public bool IsValid()
        {
            if (_recordsCache.Count <= 1)
            {
                return true;
            }
            // 取第一行的数据数为列数
            int columnSize = _recordsCache[0].Count();
            // 以第二行的数据为类型标准
            var typeList = (from i in _recordsCache[1]
                            select i.GetType()).ToArray();
            // 从第二行开始验证，每行的列数相等且同一列的数据类型相同则视为有效
            foreach (var record in _recordsCache.Skip(1))
            {
                if (record.Count() != columnSize)
                {
                    return false;
                }
                for (int i = 0; i < columnSize; i++)
                {
                    if (record.ElementAt(i).GetType() != typeList[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private readonly List<IEnumerable<Cell>> _recordsCache = new List<IEnumerable<Cell>>();
    }
}
