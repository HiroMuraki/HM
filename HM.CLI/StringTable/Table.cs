namespace HM.APManager.StringTable
{
    public class Table
    {
        public IReadOnlyCollection<string>? GetHeader() => Header;
        public IReadOnlyCollection<object?[]> GetRows() => Rows;

        internal string[]? Header { get; set; }
        internal List<object?[]> Rows { get; } = new();

        internal Table()
        {

        }
    }
}
