using System;

namespace UnityEditor
{
    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class CustomEditorAttribute : Attribute
    {
        public CustomEditorAttribute(Type type, bool val)
        {
            throw new InvalidOperationException();
        }
    }
}
