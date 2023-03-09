using System.Collections.Specialized;
using System.ComponentModel;

namespace HM.Collections.Observable
{
    [Serializable]
    public class ObservableDictionary<TKey, TValue> :
        DictionaryBase<TKey, TValue>,
        INotifyPropertyChanged,
        INotifyCollectionChanged
        where TKey : notnull
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        protected override void AddItem(KeyValuePair<TKey, TValue> keyValuePair)
        {
            base.AddItem(keyValuePair);
            CollectionChanged?.Invoke(this,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, keyValuePair));
            OnPropertyChanged(nameof(Count));
        }
        protected override void ClearItems()
        {
            var preItems = (from i in _innerDictonary select i).ToList();
            base.ClearItems();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            OnPropertyChanged(nameof(Count));
        }
        protected override bool RemoveItem(KeyValuePair<TKey, TValue> keyValuePair)
        {
            if (base.RemoveItem(keyValuePair) || _innerDictonary.Remove(keyValuePair.Key))
            {
                OnCollectionChanged(
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, keyValuePair));
                OnPropertyChanged(nameof(Count));
                return true;
            }
            else
            {
                return false;
            }
        }
        protected override void SetItem(KeyValuePair<TKey, TValue> keyValuePair)
        {
            if (_innerDictonary.TryGetValue(keyValuePair.Key, out var preValue))
            {
                base.SetItem(keyValuePair);
                OnCollectionChanged(
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, keyValuePair, KeyValuePair.Create(keyValuePair.Key, preValue)));
                OnPropertyChanged(nameof(Count));
            }
            else
            {
                AddItem(keyValuePair);
            }
        }

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            CollectionChanged?.Invoke(this, args);
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
