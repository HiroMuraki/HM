namespace HM.APManager.StringTable
{
    public record class TableGeneralElements
    {
        public static TableGeneralElements Empty { get; } = new();
        public static TableGeneralElements Default { get; } = new()
        {
            Vertical = '│',
            Horizontal = '─',

        };
        public static TableGeneralElements Bold { get; } = new()
        {
            Vertical = '┃',
            Horizontal = '━',
        };
        public static TableGeneralElements DoubleLine { get; } = new()
        {
            Vertical = '║',
            Horizontal = '═',
        };

        public char Vertical { get; init; }
        public char Horizontal { get; init; }
    }
}
