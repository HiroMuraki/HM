namespace HM.Collections.Lockable
{
    public class LockableDictionary<TKey, TValue> :
        DictionaryBase<TKey, TValue>,
        ILockableCollection<KeyValuePair<TKey, TValue>>
        where TKey : notnull
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

        protected override void AddItem(KeyValuePair<TKey, TValue> keyValuePair)
        {
            CheckAccessability();
            base.AddItem(keyValuePair);
        }
        protected override void ClearItems()
        {
            CheckAccessability();
            base.ClearItems();
        }
        protected override bool RemoveItem(KeyValuePair<TKey, TValue> keyValuePair)
        {
            CheckAccessability();
            return base.RemoveItem(keyValuePair);
        }
        protected override void SetItem(KeyValuePair<TKey, TValue> keyValuePair)
        {
            CheckAccessability();
            base.SetItem(keyValuePair);
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
