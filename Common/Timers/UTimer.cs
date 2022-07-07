namespace HM.Common.Timers
{
    public class UTimer : ITimer
    {
        public event EventHandler? Tick;

        public TimeSpan Interval { get; set; }

        public void Start()
        {
            _timer.Change(0, (int)Interval.TotalMilliseconds);
        }
        public void Stop()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }
        public void Dispose()
        {
            _timer.Dispose();
            GC.SuppressFinalize(this);
        }

        public UTimer()
        {
            _timer = new(OnTick);
        }

        private readonly Timer _timer;
        private void OnTick(object? args)
        {
            Tick?.Invoke(this, EventArgs.Empty);
        }
    }

}
