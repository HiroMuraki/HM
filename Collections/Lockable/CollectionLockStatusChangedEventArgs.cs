namespace HM.Collections.Lockable
{
    public class CollectionLockStatusChangedEventArgs : EventArgs
    {
        public LockStatus LockStatus { get; }

        public CollectionLockStatusChangedEventArgs(LockStatus status)
        {
            LockStatus = status;
        }
    }
}
