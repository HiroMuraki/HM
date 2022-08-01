namespace HM.CLI
{
    public static class ColorfulConsole
    {
        public static void WriteLine(string text, ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            ConsoleColor preForegroundColro = Console.ForegroundColor;
            ConsoleColor preBackgroundColro = Console.BackgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            Console.WriteLine(text);
            Console.ForegroundColor = preForegroundColro;
            Console.BackgroundColor = preBackgroundColro;
        }
    }
}