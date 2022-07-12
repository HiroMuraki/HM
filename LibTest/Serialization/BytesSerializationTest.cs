#pragma warning disable IDE0049
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HM.Serialization;
using HM.Debug;

namespace LibTest.Serialization
{
    class NBSFoo
    {

    }

    [BytesSerializable]
    class Foo : IEquatable<Foo>
    {
        [BytesIncluded(1)]
        public Int32 _A;
        [BytesIncluded(2)]
        public Boolean _B;
        [BytesIncluded(3)]
        public Double _D;
        [BytesIncluded(4)]
        public string _str1 = string.Empty;
        [BytesIncluded(5)]
        public string _str2 = string.Empty;
        [BytesIncluded(6)]
        public Int32[] _intArray = Array.Empty<Int32>();
        [BytesIncluded(7)]
        public string[] _strArray = Array.Empty<string>();

        public bool Equals(Foo? other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other is null || other.GetType() != GetType()) return false;

            if (_A != other._A) return false;
            if (_B != other._B) return false;
            if (_D != other._D) return false;
            if (_str1 != other._str1) return false;
            if (_str2 != other._str2) return false;

            if (!_intArray.SequenceEqual(other._intArray)) return false;
            if (!_strArray.SequenceEqual(other._strArray)) return false;

            return true;
        }

        public static Foo GetRandomOne()
        {
            var r = new Foo()
            {
                _A = RandomData.NextInt32(),
                _B = RandomData.NextBoolean(),
                _D = RandomData.NextDouble(),
                _str1 = RandomData.NextString(0, 5, CharTypes.UpperLetters),
                _str2 = RandomData.NextString(10, 100, CharTypes.All),
                _intArray = RandomData.NextInt32s(),
                _strArray = RandomData.NextStrings()
            };
            return r;
        }
    }

    [BytesSerializable]
    public class ComplexFoo : IEquatable<ComplexFoo>
    {
        // single
        [BytesIncluded(0)]
        public Byte _byteVal; // 0
        [BytesIncluded(1)]
        public SByte _sByteVal; // 1
        [BytesIncluded(2)]
        public Int16 _int16Val; // 2
        [BytesIncluded(3)]
        public UInt16 _uInt16Val; // 3
        [BytesIncluded(4)]
        public Int32 _int32Val; // 4
        [BytesIncluded(5)]
        public UInt32 _uInt32Val; // 5
        [BytesIncluded(6)]
        public Int64 _int64Val; // 6
        [BytesIncluded(7)]
        public UInt64 _uInt64Val; // 7
        [BytesIncluded(8)]
        public Single _singleVal; // 8
        [BytesIncluded(9)]
        public Double _doubleVal; // 9
        [BytesIncluded(10)]
        public Decimal _decimalVal; // 10
        [BytesIncluded(11)]
        public Boolean _booleanVal; // 11
        [BytesIncluded(12)]
        public Char _charVal; // 12
        [BytesIncluded(13)]
        public DateTime _dateTimeVal; // 13
        [BytesIncluded(14)]
        public String _stringVal = String.Empty; // 14

        [BytesIncluded(15)]
        public Byte[] _byteArray = Array.Empty<Byte>(); // 15
        [BytesIncluded(16)]
        public SByte[] _sByteArray = Array.Empty<SByte>(); // 16
        [BytesIncluded(17)]
        public Int16[] _int16Array = Array.Empty<Int16>(); // 17
        [BytesIncluded(18)]
        public UInt16[] _uInt16Array = Array.Empty<UInt16>(); // 18
        [BytesIncluded(19)]
        public Int32[] _int32Array = Array.Empty<Int32>(); // 19
        [BytesIncluded(20)]
        public UInt32[] _uInt32Array = Array.Empty<UInt32>(); // 20
        [BytesIncluded(21)]
        public Int64[] _int64Array = Array.Empty<Int64>(); // 21
        [BytesIncluded(22)]
        public UInt64[] _uInt64Array = Array.Empty<UInt64>(); // 22
        [BytesIncluded(23)]
        public Single[] _singleArray = Array.Empty<Single>(); // 23
        [BytesIncluded(24)]
        public Double[] _doubleArray = Array.Empty<Double>(); // 24
        [BytesIncluded(25)]
        public Decimal[] _decimalArray = Array.Empty<Decimal>(); // 25
        [BytesIncluded(26)]
        public Boolean[] _booleanArray = Array.Empty<Boolean>(); // 26
        [BytesIncluded(27)]
        public Char[] _charArray = Array.Empty<Char>(); // 27
        [BytesIncluded(28)]
        public DateTime[] _dateTimeArray = Array.Empty<DateTime>(); // 28
        [BytesIncluded(29)]
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

                _byteArray = RandomData.NextBytes(), // 15
                _sByteArray = RandomData.NextSBytes(), // 16
                _int16Array = RandomData.NextInt16s(), // 17
                _uInt16Array = RandomData.NextUInt16s(), // 18
                _int32Array = RandomData.NextInt32s(), // 19
                _uInt32Array = RandomData.NextUInt32s(), // 20
                _int64Array = RandomData.NextInt64s(), // 21
                _uInt64Array = RandomData.NextUInt64s(), // 22
                _singleArray = RandomData.NextSingles(), // 23
                _doubleArray = RandomData.NextDoubles(), // 24
                _decimalArray = RandomData.NextDecimals(), // 25
                _booleanArray = RandomData.NextBooleans(), // 26
                _charArray = RandomData.NextChars(), // 27
                _dateTimeArray = RandomData.NextDateTimes(), // 28
                _stringArray = RandomData.NextStrings(), // 29
            };
            return r;
        }
    }

    [BytesSerializable]
    class NestedFoo : IEquatable<NestedFoo>
    {
        [BytesIncluded(0)]
        public Foo[] _foos = Array.Empty<Foo>();
        [BytesIncluded(1)]
        public ComplexFoo[] _complexFoos = Array.Empty<ComplexFoo>();

        public static NestedFoo GetRandomOne()
        {
            return new NestedFoo()
            {
                _foos = RandomData.NextArray(() => Foo.GetRandomOne()),
                _complexFoos = RandomData.NextArray(() => ComplexFoo.GetRandomOne())
            };
        }

        public bool Equals(NestedFoo? other)
        {
            if (other is null) return false;
            if (_foos.Length != other._foos.Length) return false;
            if (_complexFoos.Length != other._complexFoos.Length) return false;

            for (int i = 0; i < _foos.Length; i++)
            {
                if (!_foos[i].Equals(other._foos[i]))
                {
                    return false;
                }
            }

            for (int i = 0; i < _complexFoos.Length; i++)
            {
                if (!_complexFoos[i].Equals(other._complexFoos[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }

    [TestClass]
    public class BytesSerializationTest
    {
        [TestMethod]
        public void SerializationTest()
        {
            BytesSerializer serializer = new BytesSerializer();

            void TestClass<T>(Func<T> objCreater) where T : notnull, IEquatable<T>
            {
                ArgumentNullException.ThrowIfNull(objCreater);

                T obj = objCreater();
                byte[] objBytes = serializer.SerializeToBytes(obj);
                T d_obj = serializer.DeserializeFromBytes<T>(objBytes);
                Assert.IsTrue(obj.Equals(d_obj));

                // 数组
                T[] array = RandomData.NextArray<T>(objCreater);
                byte[] arrayBytes = serializer.SerializeToBytes(array);
                T[] d_array = serializer.DeserializeFromBytes<T[]>(arrayBytes);
                Assert.AreEqual(array.Length, d_array.Length);
                for (int i = 0; i < array.Length; i++)
                {
                    Assert.IsTrue(array[i].Equals(d_array[i]));
                }
            }
            void TestEquals<T>(T val) where T : notnull
            {
                byte[] bytes = serializer.SerializeToBytes(val);
                T d_val = serializer.DeserializeFromBytes<T>(bytes);
                try
                {
                    Assert.AreEqual(val, d_val);
                }
                catch (AssertFailedException)
                {
                    System.Diagnostics.Debug.WriteLine($"At type: {typeof(T)}"); // debug output
                    throw;
                }
            }
            void TestArrayEqual<T>(T[] array) where T : notnull
            {
                byte[] bytesBytes = serializer.SerializeToBytes(array);
                T[] d_bytes = serializer.DeserializeFromBytes<T[]>(bytesBytes);
                try
                {
                    Assert.IsTrue(array.SequenceEqual(d_bytes));
                }
                catch (AssertFailedException)
                {
                    System.Diagnostics.Debug.WriteLine($"At type: {typeof(T)}"); // debug output
                    throw;
                }
            }

            #region 基元类型测试
            for (int _ = 0; _ < 1000; _++)
            {
                TestEquals(RandomData.NextByte());
                TestEquals(RandomData.NextSByte());
                TestEquals(RandomData.NextInt16());
                TestEquals(RandomData.NextUInt16());
                TestEquals(RandomData.NextInt32());
                TestEquals(RandomData.NextUInt32());
                TestEquals(RandomData.NextInt64());
                TestEquals(RandomData.NextUInt64());
                TestEquals(RandomData.NextSingle());
                TestEquals(RandomData.NextDouble());
                TestEquals(RandomData.NextDecimal());
                TestEquals(RandomData.NextBoolean());
                TestEquals(RandomData.NextChar());
                TestEquals(RandomData.NextDateTime());
                TestEquals(RandomData.NextString());
            }
            #endregion

            #region 数组测试
            for (int _ = 0; _ < 1000; _++)
            {
                TestArrayEqual(RandomData.NextBytes());
                TestArrayEqual(RandomData.NextSBytes());
                TestArrayEqual(RandomData.NextInt16s());
                TestArrayEqual(RandomData.NextUInt16s());
                TestArrayEqual(RandomData.NextInt32s());
                TestArrayEqual(RandomData.NextUInt32s());
                TestArrayEqual(RandomData.NextInt64s());
                TestArrayEqual(RandomData.NextUInt64s());
                TestArrayEqual(RandomData.NextSingles());
                TestArrayEqual(RandomData.NextDoubles());
                TestArrayEqual(RandomData.NextDecimals());
                TestArrayEqual(RandomData.NextBooleans());
                TestArrayEqual(RandomData.NextChars());
                TestArrayEqual(RandomData.NextDateTimes());
                TestArrayEqual(RandomData.NextStrings());
            }
            #endregion

            #region 常规引用类型测试
            for (int _ = 0; _ < 100; _++)
            {

                TestClass(Foo.GetRandomOne);
            }
            #endregion

            #region 复杂引用类型测试
            for (int _ = 0; _ < 100; _++)
            {
                TestClass(ComplexFoo.GetRandomOne);
            }
            #endregion

            #region 复杂嵌套引用类型测试
            for (int _ = 0; _ < 10; _++)
            {
                TestClass(NestedFoo.GetRandomOne);
            }
            #endregion

            #region 异常引发测试
            // 不可序列化
            Assert.ThrowsException<BytesSerializationException>(() => serializer.SerializeToBytes(new NBSFoo()));
            // 空值
            Assert.ThrowsException<BytesSerializationException>(() => serializer.SerializeToBytes(null!));
            // 含空值
            Assert.ThrowsException<BytesSerializationException>(() => serializer.SerializeToBytes(new Foo() { _str1 = null! }));
            // 多为数组
            Assert.ThrowsException<BytesSerializationException>(() => serializer.SerializeToBytes(new int[,] { { 0, 1 }, { 1, 0 } }));
            #endregion
        }
    }
}
