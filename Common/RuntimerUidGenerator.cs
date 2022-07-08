#pragma warning disable IDE0049 // 强制使用.Net CL类型名

namespace HM.Common
{
    public class RuntimerUidGenerator
    {
        public RuntimeUid Generate()
        {
            return Generate(false);
        }
        public RuntimeUid Generate(bool threadSsafe)
        {
            if (threadSsafe)
            {
                lock (_locker)
                {
                    UInt64 index = _currentIndex++;
                    return new RuntimeUid(index);
                }
            }
            else
            {
                return new RuntimeUid(_currentIndex++);
            }
        }

        private UInt64 _currentIndex;
        private readonly object _locker = new();
    }
}
