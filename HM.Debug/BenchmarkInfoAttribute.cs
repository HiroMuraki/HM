namespace HM.Debug
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class BenchmarkInfoAttribute : Attribute
    {
        public BenchmarkInfoAttribute(string title)
        {
            Title = title;
        }
        public BenchmarkInfoAttribute(string title, int cycles)
        {
            Title = title;
            Cycles = cycles;
        }

        public string Title { get; init; } = string.Empty;
        public int Cycles { get; init; } = 1;
    }
}
