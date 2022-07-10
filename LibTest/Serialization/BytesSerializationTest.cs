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
        public string _strA = string.Empty;
        [BytesIncluded(2)]
        public string _strB = string.Empty;
        [BytesIncluded(3)]
        public string _strC = string.Empty;
        [BytesIncluded(4)]
        public string _strD = string.Empty;
        [BytesIncluded(5)]
        public string _strE = string.Empty;
        [BytesIncluded(6)]
        public Int32[] _intArray = Array.Empty<Int32>();
        [BytesIncluded(7)]
        public string[] _strArray = Array.Empty<string>();

        public bool Equals(Foo? other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other is null || other.GetType() != GetType()) return false;

            return _strA == other._strA
                && _strB == other._strB
                && _strC == other._strC
                && _strD == other._strD
                && _strE == other._strE;
        }

        public static Foo GetRandomOne()
        {
            CharTypes charTypes = CharTypes.UpperLetters | CharTypes.LowerLetters | CharTypes.Symbols | CharTypes.Digits;
            var rnd = new Random();
            var r = new Foo()
            {
                _strA = RandomData.NextString(0, 5, charTypes),
                _strB = RandomData.NextString(1, 15, charTypes),
                _strC = RandomData.NextString(10, 30, charTypes),
                _strD = RandomData.NextString(10, 100, charTypes),
                _strE = RandomData.NextString(100, 255, charTypes),
            };

            int intArraySize = rnd.Next(0, 255);
            r._intArray = new int[intArraySize];
            for (int i = 0; i < intArraySize; i++)
            {
                r._intArray[i] = rnd.Next(int.MinValue, int.MaxValue);
            }

            int strArraySize = rnd.Next(0, 10);
            r._strArray = new string[strArraySize];
            for (int i = 0; i < strArraySize; i++)
            {
                r._strArray[i] = RandomData.NextString(0, 255, charTypes);
            }

            return r;
        }
        public override string ToString()
        {
            return $"{_strA}{_strB}{_strC}{_strD}{_strE}";
        }
    }

    [BytesSerializable]
    class ComplexFoo : IEquatable<ComplexFoo>
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
        public Boolean _booleanVal; // 10
        [BytesIncluded(11)]
        public Char _charVal; // 11
        [BytesIncluded(12)]
        public DateTime _dateTimeVal; // 12
        [BytesIncluded(13)]
        public String _stringVal = String.Empty; // 13
        // array
        [BytesIncluded(14)]
        public Byte[] _byteArray = Array.Empty<Byte>(); // 14
        [BytesIncluded(15)]
        public SByte[] _sByteArray = Array.Empty<SByte>(); // 15
        [BytesIncluded(16)]
        public Int16[] _int16Array = Array.Empty<Int16>(); // 16
        [BytesIncluded(17)]
        public UInt16[] _uInt16Array = Array.Empty<UInt16>(); // 17
        [BytesIncluded(18)]
        public Int32[] _int32Array = Array.Empty<Int32>(); // 18
        [BytesIncluded(19)]
        public UInt32[] _uInt32Array = Array.Empty<UInt32>(); // 19
        [BytesIncluded(20)]
        public Int64[] _int64Array = Array.Empty<Int64>(); // 20
        [BytesIncluded(21)]
        public UInt64[] _uInt64Array = Array.Empty<UInt64>(); // 21
        [BytesIncluded(22)]
        public Single[] _singleArray = Array.Empty<Single>(); // 22
        [BytesIncluded(23)]
        public Double[] _doubleArray = Array.Empty<Double>(); // 23
        [BytesIncluded(24)]
        public Boolean[] _booleanArray = Array.Empty<Boolean>(); // 24
        [BytesIncluded(25)]
        public Char[] _charArray = Array.Empty<Char>(); // 25
        [BytesIncluded(26)]
        public DateTime[] _dateTimeArray = Array.Empty<DateTime>(); // 26
        [BytesIncluded(27)]
        public String[] _stringArray = Array.Empty<String>(); // 27

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
                _booleanVal = other._booleanVal, // 10
                _charVal = other._charVal, // 11
                _dateTimeVal = other._dateTimeVal, // 12
                _stringVal = other._stringVal, // 13
                _byteArray = other._byteArray.ToArray(), // 14
                _sByteArray = other._sByteArray.ToArray(), // 15
                _int16Array = other._int16Array.ToArray(), // 16
                _uInt16Array = other._uInt16Array.ToArray(), // 17
                _int32Array = other._int32Array.ToArray(), // 18
                _uInt32Array = other._uInt32Array.ToArray(), // 19
                _int64Array = other._int64Array.ToArray(), // 20
                _uInt64Array = other._uInt64Array.ToArray(), // 21
                _singleArray = other._singleArray.ToArray(), // 22
                _doubleArray = other._doubleArray.ToArray(), // 23
                _booleanArray = other._booleanArray.ToArray(), // 24
                _charArray = other._charArray.ToArray(), // 25
                _dateTimeArray = other._dateTimeArray.ToArray(), // 26
                _stringArray = other._stringArray.ToArray(), // 27
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
            if (_booleanVal != other._booleanVal) return false; // 10
            if (_charVal != other._charVal) return false; // 11
            if (_dateTimeVal != other._dateTimeVal) return false; // 12
            if (_stringVal != other._stringVal) return false; // 13

            if (!_byteArray.SequenceEqual(other._byteArray)) return false; // 14
            if (!_sByteArray.SequenceEqual(other._sByteArray)) return false; // 15
            if (!_int16Array.SequenceEqual(other._int16Array)) return false; // 16
            if (!_uInt16Array.SequenceEqual(other._uInt16Array)) return false; // 17
            if (!_int32Array.SequenceEqual(other._int32Array)) return false; // 18
            if (!_uInt32Array.SequenceEqual(other._uInt32Array)) return false; // 19
            if (!_int64Array.SequenceEqual(other._int64Array)) return false; // 20
            if (!_uInt64Array.SequenceEqual(other._uInt64Array)) return false; // 21
            if (!_singleArray.SequenceEqual(other._singleArray)) return false; // 22
            if (!_doubleArray.SequenceEqual(other._doubleArray)) return false; // 23
            if (!_booleanArray.SequenceEqual(other._booleanArray)) return false; // 24
            if (!_charArray.SequenceEqual(other._charArray)) return false; // 25
            if (!_dateTimeArray.SequenceEqual(other._dateTimeArray)) return false; // 26
            if (!_stringArray.SequenceEqual(other._stringArray)) return false; // 27

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
                _booleanVal = RandomData.NextBoolean(), // 10
                _charVal = RandomData.NextChar(), // 11
                _dateTimeVal = RandomData.NextDateTime(), // 12
                _stringVal = RandomData.NextString(), // 13
                                                      // array
                _byteArray = RandomData.NextBytes(), // 14
                _sByteArray = RandomData.NextSBytes(), // 15
                _int16Array = RandomData.NextInt16s(), // 16
                _uInt16Array = RandomData.NextUInt16s(), // 17
                _int32Array = RandomData.NextInt32s(), // 18
                _uInt32Array = RandomData.NextUInt32s(), // 19
                _int64Array = RandomData.NextInt64s(), // 20
                _uInt64Array = RandomData.NextUInt64s(), // 21
                _singleArray = RandomData.NextSingles(), // 22
                _doubleArray = RandomData.NextDoubles(), // 23
                _booleanArray = RandomData.NextBooleans(), // 24
                _charArray = RandomData.NextChars(), // 25
                _dateTimeArray = RandomData.NextDateTimes(), // 26
                _stringArray = RandomData.NextStrings(), // 27
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

            for (int _ = 0; _ < 100; _++)
            {
                #region 基元类型测试
                Byte byteVal = RandomData.NextByte();
                byte[] byteValBytes = serializer.SerializeToBytes(byteVal);
                Byte d_byteVal = serializer.DeserializeFromBytes<Byte>(byteValBytes);
                Assert.AreEqual(byteVal, d_byteVal);

                SByte sByteVal = RandomData.NextSByte();
                byte[] sByteValBytes = serializer.SerializeToBytes(sByteVal);
                SByte d_sByteVal = serializer.DeserializeFromBytes<SByte>(sByteValBytes);
                Assert.AreEqual(sByteVal, d_sByteVal);

                Int16 int16Val = RandomData.NextInt16();
                byte[] int16ValBytes = serializer.SerializeToBytes(int16Val);
                Int16 d_int16Val = serializer.DeserializeFromBytes<Int16>(int16ValBytes);
                Assert.AreEqual(int16Val, d_int16Val);

                UInt16 uInt16Val = RandomData.NextUInt16();
                byte[] uInt16ValBytes = serializer.SerializeToBytes(uInt16Val);
                UInt16 d_uInt16Val = serializer.DeserializeFromBytes<UInt16>(uInt16ValBytes);
                Assert.AreEqual(uInt16Val, d_uInt16Val);

                Int32 int32Val = RandomData.NextInt32();
                byte[] int32ValBytes = serializer.SerializeToBytes(int32Val);
                Int32 d_int32Val = serializer.DeserializeFromBytes<Int32>(int32ValBytes);
                Assert.AreEqual(int32Val, d_int32Val);

                UInt32 uInt32Val = RandomData.NextUInt32();
                byte[] uInt32ValBytes = serializer.SerializeToBytes(uInt32Val);
                UInt32 d_uInt32Val = serializer.DeserializeFromBytes<UInt32>(uInt32ValBytes);
                Assert.AreEqual(uInt32Val, d_uInt32Val);

                Int64 int64Val = RandomData.NextInt64();
                byte[] int64ValBytes = serializer.SerializeToBytes(int64Val);
                Int64 d_int64Val = serializer.DeserializeFromBytes<Int64>(int64ValBytes);
                Assert.AreEqual(int64Val, d_int64Val);

                UInt64 uInt64Val = RandomData.NextUInt16();
                byte[] uInt64ValBytes = serializer.SerializeToBytes(uInt64Val);
                UInt64 d_uInt64Val = serializer.DeserializeFromBytes<UInt64>(uInt64ValBytes);
                Assert.AreEqual(uInt64Val, d_uInt64Val);

                Single singleVal = RandomData.NextSingle();
                byte[] singleValBytes = serializer.SerializeToBytes(singleVal);
                Single d_singleVal = serializer.DeserializeFromBytes<Single>(singleValBytes);
                Assert.AreEqual(singleVal, d_singleVal);

                Double doubleVal = RandomData.NextDouble();
                byte[] doubleValBytes = serializer.SerializeToBytes(doubleVal);
                Double d_doubleVal = serializer.DeserializeFromBytes<Double>(doubleValBytes);
                Assert.AreEqual(doubleVal, d_doubleVal);

                Boolean booleanVal = RandomData.NextBoolean();
                byte[] booleanValBytes = serializer.SerializeToBytes(booleanVal);
                Boolean d_booleanVal = serializer.DeserializeFromBytes<Boolean>(booleanValBytes);
                Assert.AreEqual(booleanVal, d_booleanVal);

                Char charVal = RandomData.NextChar();
                byte[] charValBytes = serializer.SerializeToBytes(charVal);
                Char d_charVal = serializer.DeserializeFromBytes<Char>(charValBytes);
                Assert.AreEqual(charVal, d_charVal);

                DateTime datetimeVal = RandomData.NextDateTime();
                byte[] datetimeValBytes = serializer.SerializeToBytes(datetimeVal);
                DateTime d_datetimeVal = serializer.DeserializeFromBytes<DateTime>(datetimeValBytes);
                Assert.AreEqual(datetimeVal, d_datetimeVal);

                String stringVal = RandomData.NextString();
                byte[] stringValBytes = serializer.SerializeToBytes(stringVal);
                String d_stringVal = serializer.DeserializeFromBytes<String>(stringValBytes);
                Assert.AreEqual(stringVal, d_stringVal);
                #endregion
            }

            for (int _ = 0; _ < 100; _++)
            {
                #region 数组测试
                Byte[] bytes = RandomData.NextBytes();
                byte[] bytesBytes = serializer.SerializeToBytes(bytes);
                Byte[] d_bytes = serializer.DeserializeFromBytes<Byte[]>(bytesBytes);
                Assert.IsTrue(bytes.SequenceEqual(d_bytes));

                SByte[] sBytes = RandomData.NextSBytes();
                byte[] sBytesBytes = serializer.SerializeToBytes(sBytes);
                SByte[] d_sBytes = serializer.DeserializeFromBytes<SByte[]>(sBytesBytes);
                Assert.IsTrue(sBytes.SequenceEqual(d_sBytes));

                Int16[] int16s = RandomData.NextInt16s();
                byte[] int16sBytes = serializer.SerializeToBytes(int16s);
                Int16[] d_int16s = serializer.DeserializeFromBytes<Int16[]>(int16sBytes);
                Assert.IsTrue(int16s.SequenceEqual(d_int16s));

                UInt16[] uInt16s = RandomData.NextUInt16s();
                byte[] uInt16sBytes = serializer.SerializeToBytes(uInt16s);
                UInt16[] d_uInt16s = serializer.DeserializeFromBytes<UInt16[]>(uInt16sBytes);
                Assert.IsTrue(uInt16s.SequenceEqual(d_uInt16s));

                Int32[] int32s = RandomData.NextInt32s();
                byte[] int32sBytes = serializer.SerializeToBytes(int32s);
                Int32[] d_int32s = serializer.DeserializeFromBytes<Int32[]>(int32sBytes);
                Assert.IsTrue(int32s.SequenceEqual(d_int32s));

                UInt32[] uInt32s = RandomData.NextUInt32s();
                byte[] uInt32sBytes = serializer.SerializeToBytes(uInt32s);
                UInt32[] d_uInt32s = serializer.DeserializeFromBytes<UInt32[]>(uInt32sBytes);
                Assert.IsTrue(uInt32s.SequenceEqual(d_uInt32s));

                Int64[] int64s = RandomData.NextInt64s();
                byte[] int64sBytes = serializer.SerializeToBytes(int64s);
                Int64[] d_int64s = serializer.DeserializeFromBytes<Int64[]>(int64sBytes);
                Assert.IsTrue(int64s.SequenceEqual(d_int64s));

                UInt64[] uInt64s = RandomData.NextUInt64s();
                byte[] uInt64sBytes = serializer.SerializeToBytes(uInt64s);
                UInt64[] d_uInt64s = serializer.DeserializeFromBytes<UInt64[]>(uInt64sBytes);
                Assert.IsTrue(uInt64s.SequenceEqual(d_uInt64s));

                Single[] singles = RandomData.NextSingles();
                byte[] singlesBytes = serializer.SerializeToBytes(singles);
                Single[] d_singles = serializer.DeserializeFromBytes<Single[]>(singlesBytes);
                Assert.IsTrue(singles.SequenceEqual(d_singles));

                Double[] doubles = RandomData.NextDoubles();
                byte[] doublesBytes = serializer.SerializeToBytes(doubles);
                Double[] d_doubles = serializer.DeserializeFromBytes<Double[]>(doublesBytes);
                Assert.IsTrue(doubles.SequenceEqual(d_doubles));

                Boolean[] booleans = RandomData.NextBooleans();
                byte[] boolsBytes = serializer.SerializeToBytes(booleans);
                Boolean[] d_booleans = serializer.DeserializeFromBytes<Boolean[]>(boolsBytes);
                Assert.IsTrue(booleans.SequenceEqual(d_booleans));

                Char[] chars = RandomData.NextChars();
                byte[] charsBytes = serializer.SerializeToBytes(chars);
                Char[] d_chars = serializer.DeserializeFromBytes<Char[]>(charsBytes);
                Assert.IsTrue(chars.SequenceEqual(d_chars));

                DateTime[] dateTimes = RandomData.NextDateTimes();
                byte[] datetimesBytes = serializer.SerializeToBytes(dateTimes);
                DateTime[] d_dateTimes = serializer.DeserializeFromBytes<DateTime[]>(datetimesBytes);
                Assert.IsTrue(dateTimes.SequenceEqual(d_dateTimes));

                String[] strings = RandomData.NextStrings();
                byte[] stringsBytes = serializer.SerializeToBytes(strings);
                String[] d_strings = serializer.DeserializeFromBytes<String[]>(stringsBytes);
                Assert.IsTrue(strings.SequenceEqual(d_strings));
                #endregion
            }

            for (int _ = 0; _ < 100; _++)
            {
                #region 常规引用类型测试
                Foo foo = Foo.GetRandomOne();
                byte[] fooBytes = serializer.SerializeToBytes(foo);
                Foo d_Foo = serializer.DeserializeFromBytes<Foo>(fooBytes);
                Assert.IsTrue(foo.Equals(d_Foo));
                // 数组
                Foo[] foos = RandomData.NextArray(() => Foo.GetRandomOne());
                byte[] foosBytes = serializer.SerializeToBytes(foos);
                Foo[] d_foos = serializer.DeserializeFromBytes<Foo[]>(foosBytes);
                Assert.AreEqual(foos.Length, d_foos.Length);
                for (int i = 0; i < foos.Length; i++)
                {
                    Assert.IsTrue(foos[i].Equals(d_foos[i]));
                }
                #endregion
            }

            for (int _ = 0; _ < 100; _++)
            {
                #region 复杂引用类型测试
                ComplexFoo complexFoo = ComplexFoo.GetRandomOne();
                byte[] complexFooBytes = serializer.SerializeToBytes(complexFoo);
                ComplexFoo d_complexFoo = serializer.DeserializeFromBytes<ComplexFoo>(complexFooBytes);
                Assert.IsTrue(complexFoo.Equals(d_complexFoo));

                ComplexFoo[] complexFoos = RandomData.NextArray(() => ComplexFoo.GetRandomOne());
                byte[] complexFoosBytes = serializer.SerializeToBytes(complexFoos);
                ComplexFoo[] d_complexFoos = serializer.DeserializeFromBytes<ComplexFoo[]>(complexFoosBytes);
                Assert.AreEqual(complexFoos.Length, d_complexFoos.Length);
                for (int i = 0; i < complexFoos.Length; i++)
                {
                    Assert.IsTrue(complexFoos[i].Equals(d_complexFoos[i]));
                }
                #endregion
            }

            for (int _ = 0; _ < 100; _++)
            {
                #region 复杂嵌套引用类型测试
                NestedFoo nestedFoo = NestedFoo.GetRandomOne();
                byte[] nestedFooBytes = serializer.SerializeToBytes(nestedFoo);
                NestedFoo d_complexFoo = serializer.DeserializeFromBytes<NestedFoo>(nestedFooBytes);
                Assert.IsTrue(nestedFoo.Equals(d_complexFoo));

                NestedFoo[] nestedFoos = RandomData.NextArray(() => NestedFoo.GetRandomOne());
                byte[] nestedFoosBytes = serializer.SerializeToBytes(nestedFoos);
                NestedFoo[] d_nestedFoos = serializer.DeserializeFromBytes<NestedFoo[]>(nestedFoosBytes);
                Assert.AreEqual(nestedFoos.Length, d_nestedFoos.Length);
                for (int i = 0; i < nestedFoos.Length; i++)
                {
                    Assert.IsTrue(nestedFoos[i].Equals(d_nestedFoos[i]));
                }
                #endregion
            }

            Assert.ThrowsException<BytesSerializationException>(() => serializer.SerializeToBytes(new NBSFoo()));
            Assert.ThrowsException<ArgumentNullException>(() => serializer.SerializeToBytes(null!));
            Assert.ThrowsException<NullReferenceException>(() => serializer.SerializeToBytes(new Foo() { _strA = null! }));
        }
    }
}
