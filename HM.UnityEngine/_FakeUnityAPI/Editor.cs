using System;

namespace UnityEditor
{
    public class Editor
    {
        protected SerializedObject serializedObject;

        public virtual void OnInspectorGUI()
        {
            throw new InvalidOperationException();
        }
    }
}
