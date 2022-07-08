#pragma warning disable IDE0049 // 强制使用.Net CL类型名

namespace HM.Common
{
    public readonly struct Uid : IEquatable<Uid>
    {
        public UInt64 Value => _index;

        public static bool operator ==(Uid left, Uid right)
        {
            return left._index == right._index;
        }
        public static bool operator !=(Uid left, Uid right)
        {
            return !(left == right);
        }
        public override bool Equals(object? obj)
        {
            if (obj is null || obj.GetType() != typeof(Uid)) return false;
            return Equals((Uid)obj);
        }
        public bool Equals(Uid obj)
        {
            return _index == obj._index;
        }
        public override int GetHashCode()
        {
            return _index.GetHashCode();
        }
        public static explicit operator UInt64(Uid guid)
        {
            return guid._index;
        }
        public static explicit operator Uid(UInt64 index)
        {
            return new Uid(index);
        }
        public override string ToString()
        {
            return _index.ToString("00000-00000-00000-00000");
        }

        public Uid(UInt64 index)
        {
            _index = index;
        }

        private readonly UInt64 _index;
    }
}
