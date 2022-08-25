#pragma warning disable IDE0049 // 强制使用.Net CL类型名

namespace HM.Common
{
    public interface IUidGenerator
    {
        UInt64 NextIndex { get; }
        ThreadSafeMode ThreadSafeMode { get; }

        void Forward(UInt64 step);
        void Fallback(UInt64 step);
        Uid Next();
    }
}
