namespace HM.Common.Timers
{
    public class QTimer : ITimer
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
            _isCancellationRequested = false;
            Worker();
        }
        public void Stop()
        {
            _isCancellationRequested = true;
        }
        public void Dispose()
        {
            _isCancellationRequested = true;
            GC.SuppressFinalize(this);
        }

        private bool _isCancellationRequested;
        private TimeSpan _interval = TimeSpan.FromSeconds(1);
        private async void Worker()
        {
            while (true)
            {
                if (_isCancellationRequested)
                {
                    break;
                }
                await Task.Delay(Interval);
                Tick?.Invoke(this, EventArgs.Empty);
            }
        }
    }

}
