using HM.Collections.ObservableCollection;

namespace HM.Collections.LockableCollection
{
    public class LockableDictionary<Tkey, TValue> :
        DictionaryBase<Tkey, TValue>,
        ILockableCollection<KeyValuePair<Tkey, TValue>>
        where Tkey : notnull
    {

        public event EventHandler<CollectionLockStatusChangedEventArgs>? LockStatusChanged;

        public bool IsLocked => _lockTokens.Count > 0;

        public void Lock(int token)
        {
            bool preLocked = IsLocked;
            if (_lockTokens.Add(token) && !preLocked && IsLocked)
            {
                OnCollectionLocked();
            }

        }
        public void Unlock(int token)
        {
            bool preLocked = IsLocked;
            if (_lockTokens.Remove(token) && preLocked && !IsLocked)
            {
                OnCollectionUnlocked();
            };
        }
        public void UnlockAll()
        {
            _lockTokens.Clear();
        }

        protected override void AddItem(Tkey key, TValue value)
        {
            CheckAccessability();
            base.AddItem(key, value);
        }
        protected override void ClearItems()
        {
            CheckAccessability();
            base.ClearItems();
        }
        protected override bool RemoveItem(Tkey key)
        {
            CheckAccessability();
            return base.RemoveItem(key);
        }
        protected override void SetItem(Tkey key, TValue value)
        {
            CheckAccessability();
            base.SetItem(key, value);
        }

        private readonly HashSet<int> _lockTokens = new();
        private void CheckAccessability()
        {
            if (IsLocked)
            {
                throw new InvalidOperationException("Collection was locked");
            }
        }
        private void OnCollectionLocked()
        {
            LockStatusChanged?.Invoke(this, new(LockStatus.Locked));
        }
        private void OnCollectionUnlocked()
        {
            LockStatusChanged?.Invoke(this, new(LockStatus.Unlocked));
        }
    }
}
