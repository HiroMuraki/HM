using System;

namespace HM.UnityEngine._FakeUnityAPI
{
    public class SerializedObject
    {

        public void Update() => throw new InvalidOperationException();
        public void ApplyModifiedProperties() => throw new InvalidOperationException();
        public SerializedProperty FindProperty(string name) => throw new InvalidOperationException();

    }
}
