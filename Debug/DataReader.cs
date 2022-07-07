namespace HM.Debug
{
    public static class DataReader
    {
        public static IEnumerable<string?> LinesFromFile(string file, Predicate<string?>? filter = null)
        {
            var reader = new StreamReader(file);
            return LinesFrom(reader.ReadLine, filter, f =>
            {
                if (reader.EndOfStream)
                {
                    reader.Close();
                    return true;
                };
                return false;
            }, true);
        }
        public static IEnumerable<string?> LinesFromConsole(Func<string?, bool>? eof = null, Predicate<string?>? filter = null)
        {
            return LinesFrom(Console.ReadLine, filter, eof);
        }
        public static string TextFromFile(string file)
        {
            using (var reader = new StreamReader(file))
            {
                return reader.ReadToEnd();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lineReader">Indicate how to get a new line</param>
        /// <param name="filter">Indicate if data is valid</param>
        /// <param name="eof">Indicate if eof reached</param>
        /// <param name="includedEofLine">Indicate whether preserve the last input line when eof reached</param>
        /// <returns></returns>
        public static IEnumerable<string?> LinesFrom(
            Func<string?>? lineReader = null,
            Predicate<string?>? filter = null,
            Func<string?, bool>? eof = null,
            bool includedEofLine = false)
        {

            while (true)
            {
                string? line = lineReader?.Invoke();
                if (eof is not null && eof(line))
                {
                    if (includedEofLine)
                    {
                        yield return line;
                    }
                    break;
                }
                if (filter is null || filter(line))
                {
                    yield return line;
                }
                continue;
            }
        }
    }
}
