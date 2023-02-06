namespace HM.APManager.StringTable
{
    public record class TableCornerElements
    {
        public static TableCornerElements Empty { get; } = new();
        public static TableCornerElements Default { get; } = new()
        {
            TopLeft = '┌',
            TopRight = '┐',
            BottomLeft = '└',
            BottomRight = '┘',
        };
        public static TableCornerElements Bold { get; } = new()
        {
            TopLeft = '┏',
            TopRight = '┓',
            BottomLeft = '┗',
            BottomRight = '┛',
        };
        public static TableCornerElements DoubleLine { get; } = new()
        {
            TopLeft = '╔',
            TopRight = '╗',
            BottomLeft = '╚',
            BottomRight = '╝',
        };
        public static TableCornerElements Rounded { get; } = new()
        {
            TopLeft = '╭',
            TopRight = '╮',
            BottomLeft = '╰',
            BottomRight = '╯',
        };

        public char TopLeft { get; init; }
        public char TopRight { get; init; }
        public char BottomLeft { get; init; }
        public char BottomRight { get; init; }
    }
}
