using System;

namespace UnityEngine
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
