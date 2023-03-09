using System;

namespace HM.UnityEngine._FakeUnityAPI
{
    public class Transform
    {
        public void SetParent(Transform parent)
        {
            throw new InvalidOperationException();
        }
    }
}
