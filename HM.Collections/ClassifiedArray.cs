using System.Collections;
using System.Collections.Immutable;

namespace HM.Collections
{
    public record class ClassifiedArray<TCategory, TArray> : IEnumerable<TArray> where TCategory : notnull
    {
        public static ClassifiedArray<TCategory, TArray> Empty { get; } = new();

        public TArray[] this[TCategory category] => _categories[category];

        public bool ContainsCategory(TCategory category)
        {
            return _categories.ContainsKey(category);
        }
        public bool TryGetArray(TCategory category, out TArray[]? result)
        {
            return _categories.TryGetValue(category, out result);
        }
        public IEnumerator<TArray> GetEnumerator()
        {
            foreach (var key in _categories.Keys)
            {
                foreach (var item in _categories[key])
                {
                    yield return item;
                }
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal ClassifiedArray(ImmutableDictionary<TCategory, List<TArray>> source)
        {
            _categories = source.ToImmutableDictionary(k => k.Key, v => v.Value.ToArray());
        }
        private ClassifiedArray()
        {

        }

        private readonly ImmutableDictionary<TCategory, TArray[]> _categories = ImmutableDictionary<TCategory, TArray[]>.Empty;
    }
}
