namespace HM.Debug.Extensions
{
    public static class StringExtension
    {
        public static string Format(this string self, params object?[] args)
        {
            return string.Join(self, args);
        }
        public static string Join<T>(this string self, params T[] args)
        {
            return string.Join(self, args);
        }
    }
}
