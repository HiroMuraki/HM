namespace HM.Common.Timers
{
    public sealed class Timer : ITimer
    {
        public event EventHandler? Tick;

        public TimeSpan Interval
        {
            get
            {
                return _interval;
            }
            set
            {
                _interval = value;
            }
        }

        public void Start()
        {
            Stop();
            _cts = new CancellationTokenSource();
            Worker();
        }
        public void Stop()
        {
            _cts.Cancel();
        }
        public void Dispose()
        {
            Stop();
            GC.SuppressFinalize(this);
        }

        private CancellationTokenSource _cts = new();
        private TimeSpan _interval = TimeSpan.FromSeconds(1);
        private async void Worker()
        {
            while (true)
            {
                try
                {
                    await Task.Delay(Interval, _cts.Token);
                    Tick?.Invoke(this, EventArgs.Empty);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }
    }

}
