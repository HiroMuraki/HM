#nullable enable

using HM;

namespace HM.UnityEngine.Scripts.HM
{
    public interface IPooledObject
    {
        void WhenFetch();
        void WhenReturn();
    }
}
