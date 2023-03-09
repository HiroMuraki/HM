using System;

namespace HM.UnityEngine._FakeUnityAPI
{
    public class GameObject : Object
    {
        public Transform transform;
        public GameObject gameObject;

        public void SetActive(bool active)
        {
            throw new InvalidOperationException();
        }
        public T[] GetComponents<T>()
        {
            throw new InvalidOperationException();
        }
    }
}
