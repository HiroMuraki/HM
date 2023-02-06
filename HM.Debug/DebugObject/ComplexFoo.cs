using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM.Debug
{
    public class ComplexFoo : IEquatable<ComplexFoo>
    {
        // single
        public Byte _byteVal; // 0
        public SByte _sByteVal; // 1
        public Int16 _int16Val; // 2
        public UInt16 _uInt16Val; // 3
        public Int32 _int32Val; // 4
        public UInt32 _uInt32Val; // 5
        public Int64 _int64Val; // 6
        public UInt64 _uInt64Val; // 7
        public Single _singleVal; // 8
        public Double _doubleVal; // 9
        public Decimal _decimalVal; // 10
        public Boolean _booleanVal; // 11
        public Char _charVal; // 12
        public DateTime _dateTimeVal; // 13
        public String _stringVal = String.Empty; // 14

        public Byte[] _byteArray = Array.Empty<Byte>(); // 15
        public SByte[] _sByteArray = Array.Empty<SByte>(); // 16
        public Int16[] _int16Array = Array.Empty<Int16>(); // 17
        public UInt16[] _uInt16Array = Array.Empty<UInt16>(); // 18
        public Int32[] _int32Array = Array.Empty<Int32>(); // 19
        public UInt32[] _uInt32Array = Array.Empty<UInt32>(); // 20
        public Int64[] _int64Array = Array.Empty<Int64>(); // 21
        public UInt64[] _uInt64Array = Array.Empty<UInt64>(); // 22
        public Single[] _singleArray = Array.Empty<Single>(); // 23
        public Double[] _doubleArray = Array.Empty<Double>(); // 24
        public Decimal[] _decimalArray = Array.Empty<Decimal>(); // 25
        public Boolean[] _booleanArray = Array.Empty<Boolean>(); // 26
        public Char[] _charArray = Array.Empty<Char>(); // 27
        public DateTime[] _dateTimeArray = Array.Empty<DateTime>(); // 28
        public String[] _stringArray = Array.Empty<String>(); // 29
        public static ComplexFoo CloneFrom(ComplexFoo other)
        {
            ArgumentNullException.ThrowIfNull(other);

            ComplexFoo copy = new ComplexFoo()
            {
                _byteVal = other._byteVal, // 0
                _sByteVal = other._sByteVal, // 1
                _int16Val = other._int16Val, // 2
                _uInt16Val = other._uInt16Val, // 3
                _int32Val = other._int32Val, // 4
                _uInt32Val = other._uInt32Val, // 5
                _int64Val = other._int64Val, // 6
                _uInt64Val = other._uInt64Val, // 7
                _singleVal = other._singleVal, // 8
                _doubleVal = other._doubleVal, // 9
                _decimalVal = other._decimalVal, // 10
                _booleanVal = other._booleanVal, // 11
                _charVal = other._charVal, // 12
                _dateTimeVal = other._dateTimeVal, // 13
                _stringVal = other._stringVal, // 14
                _byteArray = other._byteArray.ToArray(), // 15
                _sByteArray = other._sByteArray.ToArray(), // 16
                _int16Array = other._int16Array.ToArray(), // 17
                _uInt16Array = other._uInt16Array.ToArray(), // 18
                _int32Array = other._int32Array.ToArray(), // 19
                _uInt32Array = other._uInt32Array.ToArray(), // 20
                _int64Array = other._int64Array.ToArray(), // 21
                _uInt64Array = other._uInt64Array.ToArray(), // 22
                _singleArray = other._singleArray.ToArray(), // 23
                _doubleArray = other._doubleArray.ToArray(), // 24
                _decimalArray = other._decimalArray.ToArray(), // 25
                _booleanArray = other._booleanArray.ToArray(), // 26
                _charArray = other._charArray.ToArray(), // 27
                _dateTimeArray = other._dateTimeArray.ToArray(), // 28
                _stringArray = other._stringArray.ToArray(), // 29
            };
            return copy;
        }
        public bool Equals(ComplexFoo? other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other is null || other.GetType() != GetType()) return false;

            if (_byteVal != other._byteVal) return false; // 0
            if (_sByteVal != other._sByteVal) return false; // 1
            if (_int16Val != other._int16Val) return false; // 2
            if (_uInt16Val != other._uInt16Val) return false; // 3
            if (_int32Val != other._int32Val) return false; // 4
            if (_uInt32Val != other._uInt32Val) return false; // 5
            if (_int64Val != other._int64Val) return false; // 6
            if (_uInt64Val != other._uInt64Val) return false; // 7
            if (_singleVal != other._singleVal) return false; // 8
            if (_doubleVal != other._doubleVal) return false; // 9
            if (_decimalVal != other._decimalVal) return false; // 10
            if (_booleanVal != other._booleanVal) return false; // 11
            if (_charVal != other._charVal) return false; // 12
            if (_dateTimeVal != other._dateTimeVal) return false; // 13
            if (_stringVal != other._stringVal) return false; // 14

            if (!_byteArray.SequenceEqual(other._byteArray)) return false; // 15
            if (!_sByteArray.SequenceEqual(other._sByteArray)) return false; // 16
            if (!_int16Array.SequenceEqual(other._int16Array)) return false; // 17
            if (!_uInt16Array.SequenceEqual(other._uInt16Array)) return false; // 18
            if (!_int32Array.SequenceEqual(other._int32Array)) return false; // 19
            if (!_uInt32Array.SequenceEqual(other._uInt32Array)) return false; // 20
            if (!_int64Array.SequenceEqual(other._int64Array)) return false; // 21
            if (!_uInt64Array.SequenceEqual(other._uInt64Array)) return false; // 22
            if (!_singleArray.SequenceEqual(other._singleArray)) return false; // 23
            if (!_doubleArray.SequenceEqual(other._doubleArray)) return false; // 24
            if (!_decimalArray.SequenceEqual(other._decimalArray)) return false; // 25
            if (!_booleanArray.SequenceEqual(other._booleanArray)) return false; // 26
            if (!_charArray.SequenceEqual(other._charArray)) return false; // 27
            if (!_dateTimeArray.SequenceEqual(other._dateTimeArray)) return false; // 28
            if (!_stringArray.SequenceEqual(other._stringArray)) return false; // 29

            return true;
        }
        public static ComplexFoo GetRandomOne()
        {
            var r = new ComplexFoo()
            {
                _byteVal = RandomData.NextByte(), // 0
                _sByteVal = RandomData.NextSByte(), // 1
                _int16Val = RandomData.NextInt16(), // 2
                _uInt16Val = RandomData.NextUInt16(), // 3
                _int32Val = RandomData.NextInt32(), // 4
                _uInt32Val = RandomData.NextUInt32(), // 5
                _int64Val = RandomData.NextInt64(), // 6
                _uInt64Val = RandomData.NextUInt64(), // 7
                _singleVal = RandomData.NextSingle(), // 8
                _doubleVal = RandomData.NextDouble(), // 9
                _decimalVal = RandomData.NextDecimal(), // 10
                _booleanVal = RandomData.NextBoolean(), // 11
                _charVal = RandomData.NextChar(), // 12
                _dateTimeVal = RandomData.NextDateTime(), // 13
                _stringVal = RandomData.NextString(), // 14

                _byteArray = RandomData.Repeat(() => RandomData.NextByte(), RandomData.NextInt32(0, 20)), // 15
                _sByteArray = RandomData.Repeat(() => RandomData.NextSByte(), RandomData.NextInt32(0, 20)), // 16
                _int16Array = RandomData.Repeat(() => RandomData.NextInt16(), RandomData.NextInt32(0, 20)), // 17
                _uInt16Array = RandomData.Repeat(() => RandomData.NextUInt16(), RandomData.NextInt32(0, 20)), // 18
                _int32Array = RandomData.Repeat(() => RandomData.NextInt32(), RandomData.NextInt32(0, 20)), // 19
                _uInt32Array = RandomData.Repeat(() => RandomData.NextUInt32(), RandomData.NextInt32(0, 20)), // 20
                _int64Array = RandomData.Repeat(() => RandomData.NextInt64(), RandomData.NextInt32(0, 20)), // 21
                _uInt64Array = RandomData.Repeat(() => RandomData.NextUInt64(), RandomData.NextInt32(0, 20)), // 22
                _singleArray = RandomData.Repeat(() => RandomData.NextSingle(), RandomData.NextInt32(0, 20)), // 23
                _doubleArray = RandomData.Repeat(() => RandomData.NextDouble(), RandomData.NextInt32(0, 20)), // 24
                _decimalArray = RandomData.Repeat(() => RandomData.NextDecimal(), RandomData.NextInt32(0, 20)), // 25
                _booleanArray = RandomData.Repeat(() => RandomData.NextBoolean(), RandomData.NextInt32(0, 20)), // 26
                _charArray = RandomData.Repeat(() => RandomData.NextChar(), RandomData.NextInt32(0, 20)), // 27
                _dateTimeArray = RandomData.Repeat(() => RandomData.NextDateTime(), RandomData.NextInt32(0, 20)), // 28
                _stringArray = RandomData.Repeat(() => RandomData.NextString(), RandomData.NextInt32(0, 20)), // 29
            };
            return r;
        }
    }
}
