namespace HM.APManager.StringTable
{
    public record class TableCrossElements
    {
        public static TableCrossElements Empty { get; } = new();
        public static TableCrossElements Default { get; } = new()
        {
            Center = '┼',
            Top = '┬',
            Bottom = '┴',
            Left = '├',
            Right = '┤',
        };
        public static TableCrossElements Bold { get; } = new()
        {
            Center = '╋',
            Top = '┳',
            Bottom = '┻',
            Left = '┠',
            Right = '┫',
        };
        public static TableCrossElements DoubleLine { get; } = new()
        {
            Center = '╬',
            Top = '╦',
            Bottom = '╩',
            Left = '╠',
            Right = '╣',
        };

        public char Center { get; init; }
        public char Top { get; init; }
        public char Bottom { get; init; }
        public char Left { get; init; }
        public char Right { get; init; }
    }
}
