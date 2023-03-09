using System;

namespace HM.UnityEngine._FakeUnityAPI
{
    public class GenericMenu
    {
        public void AddItem(GUIContent a, bool b, Action c) => throw new InvalidOperationException();
        public void ShowAsContext() => throw new InvalidOperationException();
    }
}
