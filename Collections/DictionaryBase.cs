using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace HM.Collections
{
    public abstract class DictionaryBase<TKey, TValue> :
       ICollection<KeyValuePair<TKey, TValue>>,
       IEnumerable<KeyValuePair<TKey, TValue>>,
       IEnumerable,
       IDictionary<TKey, TValue>,
       IReadOnlyCollection<KeyValuePair<TKey, TValue>>,
       IReadOnlyDictionary<TKey, TValue>,
       ICollection,
       IDictionary,
       IDeserializationCallback,
       ISerializable
       where TKey : notnull
    {

        public TValue this[TKey key]
        {
            get => _innerDictonary[key];
            set => SetItem(KeyValuePair.Create(key, value));
        }
        object? IDictionary.this[object key]
        {
            get => _innerDictonary[(TKey)key];
            set => SetItem(KeyValuePair.Create((TKey)key, (TValue)value!));

        }
        public ICollection<TKey> Keys => _innerDictonary.Keys;
        public ICollection<TValue> Values => _innerDictonary.Values;
        public int Count => _innerDictonary.Count;
        public bool IsReadOnly => _innerDictonary.IsReadOnly;
        public bool IsSynchronized => ((ICollection)_innerDictonary).IsSynchronized;
        public object SyncRoot => ((ICollection)_innerDictonary).SyncRoot;
        public bool IsFixedSize => ((IDictionary)_innerDictonary).IsFixedSize;
        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => ((IReadOnlyDictionary<TKey, TValue>)_innerDictonary).Keys;
        ICollection IDictionary.Keys => ((IDictionary)_innerDictonary).Keys;
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => ((IReadOnlyDictionary<TKey, TValue>)_innerDictonary).Values;
        ICollection IDictionary.Values => ((IDictionary)_innerDictonary).Values;

        public void Add(TKey key, TValue value) => AddItem(KeyValuePair.Create(key, value));
        public void Add(KeyValuePair<TKey, TValue> item) => AddItem(item);
        public void Add(object key, object? value) => AddItem(KeyValuePair.Create((TKey)key, (TValue)value!));
        public void Clear() => ClearItems();
        public bool Contains(KeyValuePair<TKey, TValue> item) => _innerDictonary.Contains(item);
        public bool Contains(object key) => ((IDictionary)_innerDictonary).Contains(key);
        public bool ContainsKey(TKey key) => _innerDictonary.ContainsKey(key);
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => _innerDictonary.CopyTo(array, arrayIndex);
        public void CopyTo(Array array, int index) => ((ICollection)_innerDictonary).CopyTo(array, index);
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _innerDictonary.GetEnumerator();
        public void GetObjectData(SerializationInfo info, StreamingContext context) => ((ISerializable)_innerDictonary).GetObjectData(info, context);
        public void OnDeserialization(object? sender) => ((IDeserializationCallback)_innerDictonary).OnDeserialization(sender);
        public bool Remove(TKey key)
        {
            if (_innerDictonary.ContainsKey(key))
            {
                RemoveItem(KeyValuePair.Create(key, _innerDictonary[key]));
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Remove(KeyValuePair<TKey, TValue> item) => RemoveItem(item);
        public void Remove(object key) => Remove((TKey)key);
        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => _innerDictonary.TryGetValue(key, out value);
        IEnumerator IEnumerable.GetEnumerator() => _innerDictonary.GetEnumerator();
        IDictionaryEnumerator IDictionary.GetEnumerator() => ((IDictionary)_innerDictonary).GetEnumerator();

        protected virtual void AddItem(KeyValuePair<TKey, TValue> keyValuePair)
        {
            _innerDictonary.Add(keyValuePair);
        }
        protected virtual bool RemoveItem(KeyValuePair<TKey, TValue> keyValuePair)
        {
            return _innerDictonary.Remove(keyValuePair);
        }
        protected virtual void ClearItems()
        {
            _innerDictonary.Clear();
        }
        protected virtual void SetItem(KeyValuePair<TKey, TValue> keyValuePair)
        {
            if (_innerDictonary.ContainsKey(keyValuePair.Key))
            {
                _innerDictonary[keyValuePair.Key] = keyValuePair.Value;
            }
            else
            {
                AddItem(keyValuePair);
            }
        }

        protected readonly IDictionary<TKey, TValue> _innerDictonary = new Dictionary<TKey, TValue>();
    }
}
