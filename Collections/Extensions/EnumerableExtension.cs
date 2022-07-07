using System.Text;

namespace HM.Collections.Extensions
{
    public static class EnumerableExtension
    {
        public static string ToFormattedString<T>(this IEnumerable<T?> self)
        {
            var iter = self.GetEnumerator();
            var sb = new StringBuilder();
            sb.Append('[');
            if (iter.MoveNext())
            {
                while (true)
                {
                    sb.Append(iter.Current);
                    if (!iter.MoveNext())
                    {
                        break;
                    }
                    sb.Append(',');
                }
            }
            sb.Append(']');
            return sb.ToString();
        }
        public static IEnumerable<T?[]> TakeChunks<T>(this IEnumerable<T?> self, IEnumerable<int> chunkSizeIter)
        {
            if (chunkSizeIter is null)
            {
                throw new ArgumentNullException(nameof(chunkSizeIter));
            }
            if (!self.Any())
            {
                yield break;
            }

            int globalIndex = 0;
            var iter = self.GetEnumerator();

            foreach (var chunkSize in chunkSizeIter)
            {
                var chunk = new T?[chunkSize];
                int chunkIndex = 0;
                while (chunkIndex < chunkSize)
                {
                    if (!iter.MoveNext())
                    {
                        break;
                    }
                    chunk[chunkIndex] = iter.Current;
                    chunkIndex++;
                    globalIndex++;
                }
                if (chunkIndex == 0)
                {
                    yield break;
                }
                else if (chunkIndex != chunkSize)
                {
                    yield return chunk.Take(chunkIndex).ToArray();
                    yield break;
                }
                else
                {
                    yield return chunk;
                }
            };
        }
        public static IEnumerable<T?[]> AsZipped<T>(this IEnumerable<T?> self, params IEnumerable<T?>[] iters)
        {
            var startEnumerator = self.GetEnumerator();
            var enumerators = (from i in iters select i.GetEnumerator()).ToArray();

            while (true)
            {
                if (!startEnumerator.MoveNext())
                {
                    break;
                }
                var r = new T?[iters.Length + 1];
                r[0] = startEnumerator.Current;
                for (int i = 0; i < enumerators.Length; i++)
                {
                    if (!enumerators[i].MoveNext())
                    {
                        yield break;
                    };
                    r[i + 1] = enumerators[i].Current;
                }
                yield return r;
            }
        }
        public static IEnumerable<T?> Then<T>(this IEnumerable<T?> self, params IEnumerable<T?>[] iters)
        {
            foreach (var item in self)
            {
                yield return item;
            }
            foreach (var iter in iters)
            {
                foreach (var item in iter)
                {
                    yield return item;
                }
            }
        }
    }
}
