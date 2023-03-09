using System;

namespace HM.UnityEngine._FakeUnityAPI
{
    public class Object
    {
        public static T Instantiate<T>(T source)
        {
            throw new InvalidOperationException();
        }
        public static void Destroy(Object obj)
        {
            throw new InvalidOperationException();
        }
    }
}
