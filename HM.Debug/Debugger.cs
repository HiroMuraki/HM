using System.Diagnostics;
using System.Text.RegularExpressions;

namespace HM.Debug
{
    public static class Debugger
    {
        public static event Action<string>? OutputHandler;

        [Conditional("DEBUG")]
        public static void Fact(bool expr)
        {
            if (!expr)
            {
                var starckTrace = new StackTrace(true);
                var callerStarckFrame = starckTrace.GetFrame(1);
                System.Diagnostics.Debug.Assert(callerStarckFrame is not null, "Unable to track required stack info");

                int linenum = callerStarckFrame.GetFileLineNumber();
                string? srcFileName = callerStarckFrame?.GetFileName();
                System.Diagnostics.Debug.Assert(srcFileName is not null, "Unable to find required file");

                string? exprSection;
                using (var reader = new StreamReader(srcFileName))
                {
                    int currentLine = 1;
                    while (currentLine < linenum)
                    {
                        _ = reader.ReadLine();
                        currentLine++;
                    }
                    exprSection = reader.ReadLine()?.Trim();
                    System.Diagnostics.Debug.Assert(exprSection is not null, $"Unable to read expression inf from {srcFileName}");
                    exprSection = Regex.Match(exprSection, @$"(?<={nameof(Debugger)}.{nameof(Fact)}\()[\s\S]*(?=\);)").Value;
                }

                OutputHandler?.Invoke($"[{linenum}]: " + exprSection);
            }
        }

        [Conditional("DEBUG")]
        public static void Exception<T>(Action action) where T : Exception
        {
            ArgumentNullException.ThrowIfNull(action);

            try
            {
                action();
                System.Diagnostics.Debug.Assert(false, $"Excepted {typeof(T).FullName} but no exception raised");
            }
            catch (T)
            {

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Assert(false, $"Excepted {typeof(T).FullName} but {e.GetType().FullName} raised");
            }
        }
    }
}
