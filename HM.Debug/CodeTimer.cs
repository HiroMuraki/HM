using System.Diagnostics;
using System.Text;

namespace HM.Debug
{
    public static class CodeTimer
    {
        private record BenchmarkResult
        {
            private class RankComparer : IComparer<BenchmarkResult>
            {
                public int Compare(BenchmarkResult? x, BenchmarkResult? y)
                {
                    return x?.ElapsedTimes.Average().CompareTo(y?.ElapsedTimes.Average()) ?? -1;
                }
            }

            public static readonly IComparer<BenchmarkResult> DefaultRankComparer = new RankComparer();

            public string Title { get; set; } = "";
            public DateTime StartTime { get; set; }
            public DateTime FinishTime { get; set; }
            public long[] MemoryUsages { get; set; } = Array.Empty<long>();
            public double[] ElapsedTimes { get; set; } = Array.Empty<double>();

            public override string ToString()
            {
                var sb = new StringBuilder();
                sb.AppendLine($">>> Timer started at {DateTime.Now:yyyy-MM-dd:HH-mm-ss}");
                sb.AppendLine($">>> Timer stopped at {DateTime.Now:yyyy-MM-dd:HH-mm-ss}");
                sb.AppendLine($"    Avg = {ElapsedTimes.Average():F6}ms");
                sb.AppendLine($"    Max = {ElapsedTimes.Max():F6}ms");
                sb.AppendLine($"    Min = {ElapsedTimes.Min():F6}ms");
                sb.AppendLine($"    MemoryUsage = {MemoryUsages.Average()} bytes");
                return sb.ToString();
            }
        }

        public static Action<string>? OutputHandler { get; set; } = Console.WriteLine;

        public static void Run(params Action[] actions)
        {
            OutputHandler?.Invoke($">>> Code timer Started");
            var results = new List<BenchmarkResult>();
            foreach (Action action in actions)
            {
                results.Add(RunCore(action));
            }

            results.Sort(BenchmarkResult.DefaultRankComparer);
            var tableBuilder = new TableBuilder()
            {
                Padding = 3
            };
            tableBuilder.Headers = new string[] {
                "Rank",
                "Title",
                "Avg(ms)",
                "MU(bytes)",
                "Max(ms)",
                "Min(ms)",
                "Start(HH:mm:ss)",
                "Finish(HH:mm:ss)"
            };
            for (int i = 0; i < results.Count; i++)
            {
                tableBuilder.Rows.Add(new string[] {
                    (i + 1).ToString(),
                    results[i].Title,
                    results[i].ElapsedTimes.Average().ToString("F4"),
                    results[i].MemoryUsages.Average().ToString("F4"),
                    results[i].ElapsedTimes.Max().ToString("F4"),
                    results[i].ElapsedTimes.Min().ToString("F4"),
                    results[i].StartTime.ToString("HH:mm:ss"),
                    results[i].FinishTime.ToString("HH:mm:ss"),
                });
            }

            OutputHandler?.Invoke(tableBuilder.ToString()!);
            OutputHandler?.Invoke($"<<< Code timer finished");
        }

        private static BenchmarkResult RunCore(Action action)
        {
            Prepare();
            // 尝试获取BenchmarkInfoAttribute特性信息
            System.Reflection.MethodInfo methodInfo = action.Method;
            var bmAtt = Attribute.GetCustomAttribute(methodInfo, typeof(BenchmarkInfoAttribute)) as BenchmarkInfoAttribute;
            if (bmAtt is null)
            {
                bmAtt = new BenchmarkInfoAttribute("");
            }

            var stopwatch = new Stopwatch();
            var benchmarkResult = new BenchmarkResult()
            {
                ElapsedTimes = new double[bmAtt.Cycles],
                MemoryUsages = new long[bmAtt.Cycles],
            };
            benchmarkResult.Title = bmAtt.Title;
            int cycles = bmAtt.Cycles;
            // 从这里开启计时器，以求最大精度
            benchmarkResult.StartTime = DateTime.Now;
            for (int i = 0; i < cycles; i++)
            {
                long preAllocated = GC.GetTotalAllocatedBytes();
                stopwatch.Restart();
                action?.Invoke(); // 实际执行
                stopwatch.Stop();
                benchmarkResult.MemoryUsages[i] = GC.GetTotalAllocatedBytes() - preAllocated;
                benchmarkResult.ElapsedTimes[i] = stopwatch.Elapsed.TotalMilliseconds;
            }
            benchmarkResult.FinishTime = DateTime.Now;
            return benchmarkResult;
        }
        private static void Prepare()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
