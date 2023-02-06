namespace HM.CLI
{
    public class PresetConsoleOptions
    {
        public static ConsoleOptions Error { get; } = new()
        {
            ForegroundColor = ConsoleColor.Red
        };
        public static ConsoleOptions OK { get; } = new()
        {
            ForegroundColor = ConsoleColor.Green
        };
        public static ConsoleOptions Warnning { get; } = new()
        {
            ForegroundColor = ConsoleColor.DarkYellow
        };
    }
}