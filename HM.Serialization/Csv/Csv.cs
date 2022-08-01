namespace HM.Serialization.Csv
{
    public static class Csv
    {
        public static readonly char Delimiter = ',';

        public static CsvWriter Create(string csvFile)
        {
            return new CsvWriter(csvFile);
        }
        public static CsvReader Read(string csvFile)
        {
            return new CsvReader(csvFile);
        }
    }
}
