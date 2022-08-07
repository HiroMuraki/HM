#nullable enable
using UnityEngine;
using System;

namespace HM.UnityEngine
{
    [Serializable]
    public struct IntRange : IEquatable<IntRange>, IFormattable
    {
#pragma warning disable IDE1006 // Unity属性
        [SerializeField] int minimum;
        [SerializeField] int maximum;
#pragma warning restore IDE1006

        public int Minimum => minimum;
        public int Maximum => maximum;
        public int Length => maximum - minimum;

        public IntRange(int minimum, int maximum)
        {
            this.minimum = minimum;
            this.maximum = maximum;
        }

        public bool IsSubRangeOf(IntRange range)
        {
            return range.minimum <= minimum && range.maximum >= maximum;
        }
        public static bool operator ==(IntRange left, IntRange right)
        {
            return left.minimum == right.minimum && left.maximum == right.maximum;
        }
        public static bool operator !=(IntRange left, IntRange right)
        {
            return !(left == right);
        }
        public bool Equals(IntRange other)
        {
            return this == other;
        }
        public override bool Equals(object? obj)
        {
            if (obj?.GetType() != typeof(IntRange)) return false;

            return Equals((IntRange)obj);
        }
        public override int GetHashCode()
        {
            return minimum << 2 + maximum;
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