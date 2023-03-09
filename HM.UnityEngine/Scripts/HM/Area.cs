#nullable enable
using UnityEngine;
using System;
using HM.UnityEngine._FakeUnityAPI;
using HM.UnityEngine.Scripts.HM.Structs;

namespace HM.UnityEngine.Scripts.HM
{
    [Serializable]
    public class Area
    {
        [SerializeField] FloatRange widthRange;
        [SerializeField] FloatRange heightRange;
        [SerializeField] Vector2 anchor;

        public Vector2 TopLeft => new Vector2(widthRange.Minimum, heightRange.Maximum) + anchor;
        public Vector2 TopRight => new Vector2(widthRange.Maximum, heightRange.Maximum) + anchor;
        public Vector2 BottomLeft => new Vector2(widthRange.Minimum, heightRange.Minimum) + anchor;
        public Vector2 BottomRight => new Vector2(widthRange.Maximum, heightRange.Minimum) + anchor;
        public Vector2 Anchor => anchor;
        public float Width => widthRange.Maximum - widthRange.Minimum;
        public float Height => heightRange.Maximum - heightRange.Minimum;
        public Vector2 Center => new Vector2(BottomLeft.x + Width / 2, BottomLeft.y + Height / 2);
    }
}
