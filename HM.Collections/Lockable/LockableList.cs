using System.Collections.ObjectModel;

namespace HM.Collections.Lockable
{
    [Serializable]
    public class LockableList<T> : Collection<T>, ILockableCollection<T>
    {
        public event EventHandler<CollectionLockStatusChangedEventArgs>? LockStatusChanged;

        public bool IsLocked => _lockTokens.Count > 0;
        public int TokenCount => _lockTokens.Count;

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
        protected override void ClearItems()
        {
            CheckAccessability();
            base.ClearItems();
        }
        protected override void InsertItem(int index, T item)
        {
            CheckAccessability();
            base.InsertItem(index, item);
        }
        protected override void RemoveItem(int index)
        {
            CheckAccessability();
            base.RemoveItem(index);
        }
        protected override void SetItem(int index, T item)
        {
            CheckAccessability();
            base.SetItem(index, item);
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
