using System;

namespace HM.UnityEngine._FakeUnityAPI
{
    public class GUIContent
    {
        public static readonly GUIContent none;

        public GUIContent(string value) => throw new InvalidOperationException();
    }
}
