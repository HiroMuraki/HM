namespace HM.Collections.ObservableCollection
{
    public enum LockStatus
    {
        Unlocked,
        Locked
    }
    public class CollectionLockStatusChangedEventArgs : EventArgs
    {
        public LockStatus LockStatus { get; }

        public CollectionLockStatusChangedEventArgs(LockStatus status)
        {
            LockStatus = status;
        }
    }

    public class CollectionModifiedEventArgs<T> : EventArgs
    {
        public CollectionModfiyAction Action { get; }
        public T[] AffactedItems { get; } = Array.Empty<T>();

        public CollectionModifiedEventArgs(CollectionModfiyAction action, T[] affactedItems)
        {
            Action = action;
            AffactedItems = affactedItems;
        }
    }
}
