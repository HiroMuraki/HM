using System.Collections.Immutable;

namespace HM.Collections
{
    public class ClassfiedArrayBuilder<TCategory, TArray> where TCategory : notnull
    {
        public List<TArray> this[TCategory category]
        {
            get
            {
                if (!_categories.TryGetValue(category, out var result))
                {
                    result = new List<TArray>();
                    _categories[category] = result;
                }
                return result;
            }
            set
            {
                _categories[category] = value;
            }
        }

        public ClassifiedArray<TCategory, TArray> ToClassfiedArray()
        {
            return new ClassifiedArray<TCategory, TArray>(_categories.ToImmutableDictionary());
        }

        private readonly Dictionary<TCategory, List<TArray>> _categories = new();
    }
}
