using System;

namespace HM.UnityEngine._FakeUnityAPI
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
