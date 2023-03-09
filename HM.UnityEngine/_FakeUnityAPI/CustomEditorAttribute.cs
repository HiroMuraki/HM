using System;

namespace HM.UnityEngine._FakeUnityAPI
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class CustomEditorAttribute : Attribute
    {
        public CustomEditorAttribute(Type type, bool val)
        {
            throw new InvalidOperationException();
        }
    }
}
