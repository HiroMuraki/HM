#nullable enable

namespace HM.UnityEngine
{
    public interface IPooledObject
    {
        void WhenFetch();
        void WhenReturn();
    }
}
