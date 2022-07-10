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
        [BytesIncluded(0)]
        public Byte _byteVal;
        [BytesIncluded(1)]
        public SByte _sByteVal;
        [BytesIncluded(2)]
        public Int16 _int16Val;
        [BytesIncluded(3)]
        public UInt16 _uInt16Val;
        [BytesIncluded(4)]
        public Int32 _int32Val;
        [BytesIncluded(5)]
        public UInt32 _uInt32Val;
        [BytesIncluded(6)]
        public Int64 _int64Val;
        [BytesIncluded(7)]
        public UInt64 _uInt64Val;
        [BytesIncluded(8)]
        public Single _singleVal;
        [BytesIncluded(9)]
        public Double _doubleVal;
        [BytesIncluded(10)]
        public Boolean _booleanVal;
        [BytesIncluded(11)]
        public Char _charVal;
        [BytesIncluded(12)]
        public String _stringVal = String.Empty;
        [BytesIncluded(13)]
        public Foo[] _foos = Array.Empty<Foo>();

        public static ComplexFoo GetRandomOne()
        {
            var rnd = new Random();
            var r = new ComplexFoo()
            {
                _byteVal = RandomData.NextByte(),
                _sByteVal = RandomData.NextSByte(),
                _int16Val = RandomData.NextInt16(),
                _uInt16Val = RandomData.NextUInt16(),
                _int32Val = RandomData.NextInt32(),
                _uInt32Val = RandomData.NextUInt32(),
                _int64Val = RandomData.NextInt64(),
                _uInt64Val = RandomData.NextUInt64(),
                _singleVal = RandomData.NextSingle(),
                _doubleVal = RandomData.NextDouble(),
                _booleanVal = RandomData.NextBoolean(),
                _charVal = RandomData.NextChar(),
                _stringVal = RandomData.NextString()
            };
            r._foos = new Foo[rnd.Next(0, 100)];
            for (int i = 0; i < r._foos.Length; i++)
            {
                r._foos[i] = Foo.GetRandomOne();
            }

            return r;
        }
        public bool Equals(ComplexFoo? other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other is null || other.GetType() != GetType()) return false;

            bool r =
                _byteVal == other._byteVal
                && _sByteVal == other._sByteVal
                && _int16Val == other._int16Val
                && _uInt16Val == other._uInt16Val
                && _int32Val == other._int32Val
                && _uInt32Val == other._uInt32Val
                && _int64Val == other._int64Val
                && _uInt64Val == other._uInt64Val
                && _singleVal == other._singleVal
                && _doubleVal == other._doubleVal
                && _charVal == other._charVal
                && _stringVal == other._stringVal
                && _foos.Length == other._foos.Length;

            if (!r) return false;

            for (int i = 0; i < _foos.Length; i++)
            {
                if (!_foos[i].Equals(other._foos[i]))
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

            Assert.ThrowsException<BytesSerializationException>(() => serializer.SerializeToBytes(new NBSFoo()));
            Assert.ThrowsException<ArgumentNullException>(() => serializer.SerializeToBytes(null!));
            Assert.ThrowsException<NullReferenceException>(() => serializer.SerializeToBytes(new Foo() { _strA = null! }));
        }
    }
}
