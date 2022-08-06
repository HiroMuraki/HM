#nullable enable
using System;
using UnityEngine;

namespace HM.UnityEngine
{
    [Serializable]
    public struct Margin
    {
        [SerializeField] float top;
        [SerializeField] float bottom;
        [SerializeField] float left;
        [SerializeField] float right;

        public float Top
        {
            get
            {
                return top;
            }
            set
            {
                top = value;
            }
        }
        public float Bottom
        {
            get
            {
                return bottom;
            }
            set
            {
                bottom = value;
            }
        }
        public float Left
        {
            get
            {
                return left;
            }
            set
            {
                left = value;
            }
        }
        public float Right
        {
            get
            {
                return right;
            }
            set
            {
                right = value;
            }
        }

        public Margin(float left, float top, float right, float bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }
        public Margin(float value)
        {
            left = value;
            top = value;
            right = value;
            bottom = value;
        }

    }
}
