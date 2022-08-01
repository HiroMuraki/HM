namespace HM.Collections.Lockable
{
    public enum LockStatus
    {
        Unlocked,
        Locked
    }

    /// <summary>
    /// Represent a collection that can be locked to prevent any modifications to the collection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ILockableCollection<T>
    {
        event EventHandler<CollectionLockStatusChangedEventArgs>? LockStatusChanged;

        void Lock(int token);
        void Unlock(int token);

        bool IsLocked { get; }
    }
}
