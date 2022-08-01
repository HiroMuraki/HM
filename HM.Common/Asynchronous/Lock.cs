namespace HM.Common.Asynchronous
{
    public sealed class Lock
    {
        public bool IsLocked { get; private set; }

        public void Enter()
        {
            lock (_locker)
            {
                IsLocked = true;
            }
        }
        public void Exit()
        {
            lock (_locker)
            {
                IsLocked = false;
            }
        }

        private readonly object _locker = new();
    }
}
