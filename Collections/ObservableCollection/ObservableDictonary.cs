using Collections;

namespace HM.Collections.ObservableCollection
{
    [Serializable]
    public class ObservableDictonary<TKey, TValue> :
        DictionaryBase<TKey, TValue>,
        IObservableCollection<KeyValuePair<TKey, TValue>> where TKey : notnull
    {

        public event EventHandler<CollectionModifiedEventArgs<KeyValuePair<TKey, TValue>>>? PreviewCollectionModified;
        public event EventHandler<CollectionModifiedEventArgs<KeyValuePair<TKey, TValue>>>? CollectionModified;

        public NotificationMode NotificationMode { get; set; } = NotificationMode.All;

        protected override void AddItem(TKey key, TValue value)
        {
            if (!_innerDictonary.ContainsKey(key))
            {
                OnPreviewDictonaryChanged(CollectionModfiyAction.Add, new KeyValuePair<TKey, TValue>(key, value));
            }
            base.AddItem(key, value);
            OnDictonaryChanged(CollectionModfiyAction.Add, key);
        }
        protected override void ClearItems()
        {
            var preItems = (from i in _innerDictonary select i).ToArray();
            OnPreviewDictonaryChanged(CollectionModfiyAction.Remove, preItems);
            base.ClearItems();
            OnDictonaryChanged(CollectionModfiyAction.Remove, preItems);
        }
        protected override bool RemoveItem(TKey key)
        {
            if (_innerDictonary.ContainsKey(key))
            {
                var removedItem = new KeyValuePair<TKey, TValue>(key, _innerDictonary[key]);
                OnPreviewDictonaryChanged(CollectionModfiyAction.Remove, removedItem);
                if (base.RemoveItem(key))
                {
                    OnDictonaryChanged(CollectionModfiyAction.Remove, removedItem);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        protected override void SetItem(TKey key, TValue value)
        {
            bool haskey = _innerDictonary.ContainsKey(key);
            var action = haskey ? CollectionModfiyAction.Update : CollectionModfiyAction.Add;
            OnPreviewDictonaryChanged(action, new KeyValuePair<TKey, TValue>(key, value));
            base.SetItem(key, value);
            OnDictonaryChanged(action, key);
        }

        private void OnPreviewDictonaryChanged(CollectionModfiyAction action, TKey key)
        {
            OnPreviewDictonaryChanged(action, new KeyValuePair<TKey, TValue>[] {
                new(key, _innerDictonary[key])
            });
        }
        private void OnPreviewDictonaryChanged(CollectionModfiyAction action, KeyValuePair<TKey, TValue> affactedItem)
        {
            OnPreviewDictonaryChanged(action, new KeyValuePair<TKey, TValue>[] { affactedItem });
        }
        private void OnPreviewDictonaryChanged(CollectionModfiyAction action, KeyValuePair<TKey, TValue>[] affactedItems)
        {
            if ((NotificationMode & NotificationMode.Preview) == NotificationMode.Preview)
            {
                PreviewCollectionModified?.Invoke(this, new(action, affactedItems));
            }
        }
        private void OnDictonaryChanged(CollectionModfiyAction action, TKey key)
        {
            OnDictonaryChanged(action, new KeyValuePair<TKey, TValue>[] {
                new(key, _innerDictonary[key])
            });
        }
        private void OnDictonaryChanged(CollectionModfiyAction action, KeyValuePair<TKey, TValue> affactedItem)
        {
            OnDictonaryChanged(action, new KeyValuePair<TKey, TValue>[] { affactedItem });
        }
        private void OnDictonaryChanged(CollectionModfiyAction action, KeyValuePair<TKey, TValue>[] affactedItems)
        {
            if ((NotificationMode & NotificationMode.Normal) == NotificationMode.Normal)
            {
                CollectionModified?.Invoke(this, new(action, affactedItems));
            }
        }
    }
}
