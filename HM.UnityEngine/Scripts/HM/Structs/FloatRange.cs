#nullable enable
using UnityEngine;
using System;

namespace HM.UnityEngine
{
    [Serializable]
    public struct FloatRange : IEquatable<FloatRange>, IFormattable
    {
#pragma warning disable IDE1006 // Unity属性
        [SerializeField] float minimum;
        [SerializeField] float maximum;
#pragma warning restore IDE1006

        public float Minimum => minimum;
        public float Maximum => maximum;
        public float Length => maximum - minimum;

        public FloatRange(float minimum, float maximum)
        {
            this.minimum = minimum;
            this.maximum = maximum;
        }

        public bool IsSubRangeOf(FloatRange range)
        {
            return range.minimum <= minimum && range.maximum >= maximum;
        }
        public static bool operator ==(FloatRange left, FloatRange right)
        {
            return left.minimum == right.minimum && left.maximum == right.maximum;
        }
        public static bool operator !=(FloatRange left, FloatRange right)
        {
            return !(left == right);
        }
        public bool Equals(FloatRange other)
        {
            return this == other;
        }
        public override bool Equals(object? obj)
        {
            if (obj?.GetType() != typeof(FloatRange)) return false;

            return Equals((FloatRange)obj);
        }
        public override int GetHashCode()
        {
            return (int)minimum << 2 + (int)maximum;
        }
        public override string ToString()
        {
            return $"[{minimum}..{maximum}]";
        }
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return $"[{minimum.ToString(format, formatProvider)}..{maximum.ToString(format, formatProvider)}]";
        }
    }
}
