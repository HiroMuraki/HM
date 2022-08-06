#nullable enable
using UnityEngine;
using System;

namespace HM.UnityEngine
{
    [Serializable]
    public struct IntRange
    {
        [SerializeField] int minValue;
        [SerializeField] int maxValue;

        public IntRange(int minValue, int maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        public int MinValue => minValue;
        public int MaxValue => maxValue;
    }
}
