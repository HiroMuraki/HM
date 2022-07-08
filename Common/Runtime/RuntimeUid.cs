#pragma warning disable IDE0049 // 强制使用.Net CL类型名

namespace HM.Common.Runtime
{
    public readonly struct RuntimeUid : IEquatable<RuntimeUid>
    {
        public UInt64 Value => _index;

        public static RuntimeUid NewGuid()
        {
            return NewGuid(false);
        }
        public static RuntimeUid NewGuid(bool threadSsafe)
        {
            if (threadSsafe)
            {
                lock (_locker)
                {
                    UInt64 index = _globalNextIndex++;
                    return new RuntimeUid(index);
                }
            }
            else
            {
                return new RuntimeUid(_globalNextIndex++);
            }
        }
        public static bool operator ==(RuntimeUid left, RuntimeUid right)
        {
            return left._index == right._index;
        }
        public static bool operator !=(RuntimeUid left, RuntimeUid right)
        {
            return !(left == right);
        }
        public override bool Equals(object? obj)
        {
            if (obj is null || obj.GetType() != typeof(RuntimeUid)) return false;
            return Equals((RuntimeUid)obj);
        }
        public bool Equals(RuntimeUid obj)
        {
            return _index == obj._index;
        }
        public override int GetHashCode()
        {
            return _index.GetHashCode();
        }
        public static explicit operator UInt64(RuntimeUid guid)
        {
            return guid._index;
        }
        public static explicit operator RuntimeUid(UInt64 index)
        {
            return new RuntimeUid(index);
        }
        public override string ToString()
        {
            return _index.ToString("00000-00000-00000-00000");
        }

        public RuntimeUid(UInt64 index)
        {
            _index = index;
        }

        private readonly UInt64 _index;
        private static UInt64 _globalNextIndex;
        private static readonly object _locker = new();
    }
}
