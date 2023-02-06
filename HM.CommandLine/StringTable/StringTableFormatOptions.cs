using System;

namespace HM.CommandLine.StringTable
{
    public record class StringTableFormatOptions
    {
        public static StringTableFormatOptions Default { get; }
        public static StringTableFormatOptions Bold { get; }
        public static StringTableFormatOptions DoubleLine { get; }
        public static StringTableFormatOptions Rounded { get; }

        public Dictionary<string, string> HeaderConverters { get; init; } = new();
        public Dictionary<Type, Func<object?, string>> ValueConverters { get; init; } = new();
        public string CellLeftPadding { get; init; } = string.Empty;
        public string CellRightPadding { get; init; } = string.Empty;
        public CellAlignment CellAlignment { get; init; } = CellAlignment.Left;
        public TableGeneralElements TableGeneralElements { get; init; } = TableGeneralElements.Default;
        public TableCornerElements TableCornerElements { get; init; } = TableCornerElements.Default;
        public TableCrossElements TableCrossElements { get; init; } = TableCrossElements.Default;

        static StringTableFormatOptions()
        {

            Default = new()
            {
                TableGeneralElements = TableGeneralElements.Default,
                TableCornerElements = TableCornerElements.Default,
                TableCrossElements = TableCrossElements.Default
            };
            Bold = new()
            {
                TableGeneralElements = TableGeneralElements.Bold,
                TableCornerElements = TableCornerElements.Bold,
                TableCrossElements = TableCrossElements.Bold
            };
            DoubleLine = new()
            {
                TableGeneralElements = TableGeneralElements.DoubleLine,
                TableCornerElements = TableCornerElements.DoubleLine,
                TableCrossElements = TableCrossElements.DoubleLine
            };
            Rounded = Default with
            {
                TableCornerElements = TableCornerElements.Rounded,
            };
        }
    }
}
