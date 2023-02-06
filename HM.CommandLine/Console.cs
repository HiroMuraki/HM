namespace HM.CommandLine
{
    public class Console
    {
        public static Console Instance { get; } = new();

        public TextWriter Out { get; } = System.Console.Out;
        public TextWriter Error { get; } = System.Console.Out;
        public bool IsOutputRedirected => System.Console.IsOutputRedirected;
        public bool IsErrorRedirected => System.Console.IsErrorRedirected;
        public bool IsInputRedirected => System.Console.IsInputRedirected;

        public void Write(string? value)
        {
            Out.Write(value!);
        }
        public void Write(string? value, ConsoleOptions options)
        {
            var preOption = GetPreviousOptions(options);
            ApplyOptions(options);
            Out.Write(value!);
            ApplyOptions(preOption);
        }
        public void Write(object? value)
        {
            Write(value?.ToString());
        }
        public void Write(object? value, ConsoleOptions options)
        {
            Write(value?.ToString(), options);
        }
        public void WriteLine()
        {
            Out.WriteLine(string.Empty);
        }
        public void WriteLine(string? value)
        {
            Out.WriteLine(value!);
        }
        public void WriteLine(string? value, ConsoleOptions options)
        {
            var preOption = GetPreviousOptions(options);
            ApplyOptions(options);
            Out.WriteLine(value!);
            ApplyOptions(preOption);
        }
        public void WriteLine(object? value)
        {
            WriteLine(value?.ToString());
        }
        public void WriteLine(object? value, ConsoleOptions options)
        {
            WriteLine(value?.ToString(), options);
        }

        private Console()
        {

        }
        private ConsoleOptions GetPreviousOptions(ConsoleOptions options)
        {
            var preOptions = new ConsoleOptions();

            if (options.ForegroundColor.HasValue)
                preOptions.ForegroundColor = System.Console.ForegroundColor;

            if (options.BackgroundColor.HasValue)
                preOptions.BackgroundColor = System.Console.BackgroundColor;

            return preOptions;
        }
        private void ApplyOptions(ConsoleOptions options)
        {
            if (options.ForegroundColor.HasValue)
                System.Console.ForegroundColor = options.ForegroundColor.Value;

            if (options.BackgroundColor.HasValue)
                System.Console.BackgroundColor = options.BackgroundColor.Value;
        }
    }
}