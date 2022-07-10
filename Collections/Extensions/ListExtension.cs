namespace HM.Collections.Extensions
{
    public static class ListExtension
    {
        public static void RemoveIf<T>(this IList<T> self, Predicate<T> predicate)
        {
            ArgumentNullException.ThrowIfNull(predicate);

            for (int i = 0; i < self.Count; i++)
            {
                if (predicate(self[i]))
                {
                    self.RemoveAt(i);
                    --i;
                }
            }
        }
    }
}
