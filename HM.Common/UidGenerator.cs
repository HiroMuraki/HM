#pragma warning disable IDE0049 // 强制使用.Net CL类型名
using System.Runtime.CompilerServices;

namespace HM.Common
{
    public enum ThreadSafeMode
    {
        NoThreadSafe,
        EnsureThreadSafe
    }
    public sealed class UidGenerator : IUidGenerator
    {
        public static UidGenerator Default { get; } = new();

        public UInt64 NextIndex => _nextIndex;
        public ThreadSafeMode ThreadSafeMode => _threadSafeMode;

        public void Forward(UInt64 step)
        {
            if (_threadSafeMode == ThreadSafeMode.EnsureThreadSafe)
            {
                lock (_locker)
                {
                    _nextIndex += step;
                }
            }
            else
            {
                _nextIndex += step;
            }
        }
        public void Fallback(UInt64 step)
        {
            if (_threadSafeMode == ThreadSafeMode.EnsureThreadSafe)
            {
                lock (_locker)
                {
                    FallbackHelper(step);
                }
            }
            else
            {
                FallbackHelper(step);
            }
        }
        public Uid Next()
        {
            if (_threadSafeMode == ThreadSafeMode.EnsureThreadSafe)
            {
                lock (_locker)
                {
                    UInt64 index = _nextIndex;
                    _nextIndex++;
                    return new Uid(index);
                }
            }
            else
            {
                UInt64 index = _nextIndex;
                _nextIndex++;
                return new Uid(index);
            }
        }

        public UidGenerator() : this(0ul, ThreadSafeMode.NoThreadSafe) { }
        public UidGenerator(UInt64 startIndex) : this(startIndex, ThreadSafeMode.NoThreadSafe) { }
        public UidGenerator(UInt64 startIndex, ThreadSafeMode threadSafeMode)
        {
            _startIndex = startIndex;
            _nextIndex = startIndex;
            _threadSafeMode = threadSafeMode;
        }

        private UInt64 _nextIndex;
        private readonly ThreadSafeMode _threadSafeMode;
        private readonly UInt64 _startIndex;
        private readonly object _locker = new();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void FallbackHelper(UInt64 step)
        {
            if (_nextIndex == _startIndex)
            {
                throw new ArgumentOutOfRangeException(nameof(step), "Next uid already equals start uid");
            }
            _nextIndex -= step;
        }
    }
}
