#pragma warning disable IDE0049 // 强制使用.Net CL类型名
using System.Runtime.CompilerServices;

namespace HM.Common
{
    public sealed class UidGenerator
    {
        public static UidGenerator Default { get; } = new();

        public UInt64 NextIndex => _nextIndex;
        public bool ThreadSafe => _threadSafe;

        public void Forward(UInt64 step)
        {
            if (_threadSafe)
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
            if (_threadSafe)
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
            if (_threadSafe)
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

        public UidGenerator() : this(0ul, false) { }
        public UidGenerator(UInt64 startIndex) : this(startIndex, false) { }
        public UidGenerator(UInt64 startIndex, bool threadSafe)
        {
            _startIndex = startIndex;
            _nextIndex = startIndex;
            _threadSafe = threadSafe;
        }

        private UInt64 _nextIndex;
        private readonly bool _threadSafe;
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
