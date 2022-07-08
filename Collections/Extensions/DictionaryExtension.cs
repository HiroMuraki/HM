namespace HM.Collections.Extensions
{
    public static class DictionaryExtension
    {
        public static string ToFormattedString<TKey, TValue>(this IDictionary<TKey, TValue> self, string? keyFormat = null, string? valueFormat = null) where TKey : notnull
        {
            var properties = new List<string>();
            foreach (var item in self)
            {
                string? fKey = item.Key.ToString();
                string? fValue = item.Key.ToString();
                properties.Add($"{fKey}: {fValue}");
            }
            return $"{{{string.Join(", ", properties)}}}";
        }
        public static IEnumerable<KeyValuePair<TValue, TKey>> ToKeyValuePairReversed<TKey, TValue>(this IDictionary<TKey, TValue> self)
        {
            foreach (var item in self)
            {
                yield return new KeyValuePair<TValue, TKey>(item.Value, item.Key);
            }
        }
    }
}
