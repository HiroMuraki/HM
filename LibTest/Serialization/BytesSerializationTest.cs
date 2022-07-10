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
                _strA = RandomData.RandomString(0, 5, charTypes),
                _strB = RandomData.RandomString(1, 15, charTypes),
                _strC = RandomData.RandomString(10, 30, charTypes),
                _strD = RandomData.RandomString(10, 100, charTypes),
                _strE = RandomData.RandomString(100, 255, charTypes),
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
                r._strArray[i] = RandomData.RandomString(0, 255, charTypes);
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
                _byteVal = RandomData.RandomByte(),
                _sByteVal = RandomData.RandomSByte(),
                _int16Val = RandomData.RandomInt16(),
                _uInt16Val = RandomData.RandomUInt16(),
                _int32Val = RandomData.RandomInt32(),
                _uInt32Val = RandomData.RandomUInt32(),
                _int64Val = RandomData.RandomInt64(),
                _uInt64Val = RandomData.RandomUInt64(),
                _singleVal = RandomData.RandomSingle(),
                _doubleVal = RandomData.RandomDouble(),
                _booleanVal = RandomData.RandomBoolean(),
                _charVal = RandomData.RandomChar(),
                _stringVal = RandomData.RandomString()
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

            #region 基元类型测试
            Byte byteVal = 163;
            byte[] byteValBytes = serializer.SerializeToBytes(byteVal);
            Byte d_byteVal = serializer.DeserializeFromBytes<Byte>(byteValBytes);
            Assert.AreEqual(byteVal, d_byteVal);

            SByte sByteVal = -121;
            byte[] sByteValBytes = serializer.SerializeToBytes(sByteVal);
            SByte d_sByteVal = serializer.DeserializeFromBytes<SByte>(sByteValBytes);
            Assert.AreEqual(sByteVal, d_sByteVal);

            Int16 int16Val = -19366;
            byte[] int16ValBytes = serializer.SerializeToBytes(int16Val);
            Int16 d_int16Val = serializer.DeserializeFromBytes<Int16>(int16ValBytes);
            Assert.AreEqual(int16Val, d_int16Val);

            UInt16 uInt16Val = 11460;
            byte[] uInt16ValBytes = serializer.SerializeToBytes(uInt16Val);
            UInt16 d_uInt16Val = serializer.DeserializeFromBytes<UInt16>(uInt16ValBytes);
            Assert.AreEqual(uInt16Val, d_uInt16Val);

            Int32 int32Val = 2139080733;
            byte[] int32ValBytes = serializer.SerializeToBytes(int32Val);
            Int32 d_int32Val = serializer.DeserializeFromBytes<Int32>(int32ValBytes);
            Assert.AreEqual(int32Val, d_int32Val);

            UInt32 uInt32Val = 1347079290;
            byte[] uInt32ValBytes = serializer.SerializeToBytes(uInt32Val);
            UInt32 d_uInt32Val = serializer.DeserializeFromBytes<UInt32>(uInt32ValBytes);
            Assert.AreEqual(uInt32Val, d_uInt32Val);

            Int64 int64Val = 7219937555865006422;
            byte[] int64ValBytes = serializer.SerializeToBytes(int64Val);
            Int64 d_int64Val = serializer.DeserializeFromBytes<Int64>(int64ValBytes);
            Assert.AreEqual(int64Val, d_int64Val);

            UInt64 uInt64Val = 7253193780593148584;
            byte[] uInt64ValBytes = serializer.SerializeToBytes(uInt64Val);
            UInt64 d_uInt64Val = serializer.DeserializeFromBytes<UInt64>(uInt64ValBytes);
            Assert.AreEqual(uInt64Val, d_uInt64Val);

            Single singleVal = 1599386921.3605106f;
            byte[] singleValBytes = serializer.SerializeToBytes(singleVal);
            Single d_singleVal = serializer.DeserializeFromBytes<Single>(singleValBytes);
            Assert.AreEqual(singleVal, d_singleVal);

            Double doubleVal = 1.7306591177174843e+18;
            byte[] doubleValBytes = serializer.SerializeToBytes(doubleVal);
            Double d_doubleVal = serializer.DeserializeFromBytes<Double>(doubleValBytes);
            Assert.AreEqual(doubleVal, d_doubleVal);

            Char charVal = 'W';
            byte[] charValBytes = serializer.SerializeToBytes(charVal);
            Char d_charVal = serializer.DeserializeFromBytes<Char>(charValBytes);
            Assert.AreEqual(charVal, d_charVal);

            String stringVal = "Hello World!";
            byte[] stringValBytes = serializer.SerializeToBytes(stringVal);
            String d_stringVal = serializer.DeserializeFromBytes<String>(stringValBytes);
            Assert.AreEqual(stringVal, d_stringVal);
            #endregion

            #region 数组测试
            Byte[] bytes = { 68, 218, 146, 70, 130, 39, 204, 72, 4, 128, 74, 202, 32, 68 };
            byte[] bytesBytes = serializer.SerializeToBytes(bytes);
            Byte[] d_bytes = serializer.DeserializeFromBytes<Byte[]>(bytesBytes);
            Assert.IsTrue(bytes.SequenceEqual(d_bytes));

            SByte[] sBytes = { 83, 7, 12, 124, -53, 102, 45, 115, -107, -1, 83, 39 };
            byte[] sBytesBytes = serializer.SerializeToBytes(sBytes);
            SByte[] d_sBytes = serializer.DeserializeFromBytes<SByte[]>(sBytesBytes);
            Assert.IsTrue(sBytes.SequenceEqual(d_sBytes));

            Int16[] int16s = { 6112, -24744, 29905, -23286, -1156, 27273 };
            byte[] int16sBytes = serializer.SerializeToBytes(int16s);
            Int16[] d_int16s = serializer.DeserializeFromBytes<Int16[]>(int16sBytes);
            Assert.IsTrue(int16s.SequenceEqual(d_int16s));

            UInt16[] uInt16s = { };
            byte[] uInt16sBytes = serializer.SerializeToBytes(uInt16s);
            UInt16[] d_uInt16s = serializer.DeserializeFromBytes<UInt16[]>(uInt16sBytes);
            Assert.IsTrue(uInt16s.SequenceEqual(d_uInt16s));

            Int32[] int32s = { -518528559, -275514502, 1296960094, 461848244, 1454431991, -1213127808, -1599492173, 1315200804, -1802540848 };
            byte[] int32sBytes = serializer.SerializeToBytes(int32s);
            Int32[] d_int32s = serializer.DeserializeFromBytes<Int32[]>(int32sBytes);
            Assert.IsTrue(int32s.SequenceEqual(d_int32s));

            UInt32[] uInt32s = { 3417456918, 600913353, 1224006045, 2456145301, 3593845788, 678712697, 3669136371, 156634776, 2425693351, 1861310382, 1318723941, 2626039393, 1451776097 };
            byte[] uInt32sBytes = serializer.SerializeToBytes(uInt32s);
            UInt32[] d_uInt32s = serializer.DeserializeFromBytes<UInt32[]>(uInt32sBytes);
            Assert.IsTrue(uInt32s.SequenceEqual(d_uInt32s));

            Int64[] int64s = { 705629920443296814, -3743587493018062743, -6373637354553219854, -9194732839102503877, 7722816616071883700, 4220936444568252085, -3664763509496345621, 981816101019960889, 7709108999588613492, 4095311424549278944, -6526373629924251535, 5118233986691655629, 5971448312443886571, 6880910501566746647, -2355676551441473681 };
            byte[] int64sBytes = serializer.SerializeToBytes(int64s);
            Int64[] d_int64s = serializer.DeserializeFromBytes<Int64[]>(int64sBytes);
            Assert.IsTrue(int64s.SequenceEqual(d_int64s));

            UInt64[] uInt64s = { 2965986654207054110, 810962725894564118, 3818120099948496103, 4794334868012787251, 10262304274147361494, 9574963725927890167, 2224327107129470609, 5054778635718097457, 13688564685785789234, 17135708353249666363, 16368649944320756076 };
            byte[] uInt64sBytes = serializer.SerializeToBytes(uInt64s);
            UInt64[] d_uInt64s = serializer.DeserializeFromBytes<UInt64[]>(uInt64sBytes);
            Assert.IsTrue(uInt64s.SequenceEqual(d_uInt64s));

            Single[] singles = { 254165919f, 821076475f, 1451407828f, 1668875753f, 1648600361f, 845043702f, 245075316f, 184188084f, 1127854649f, 625407581f, 783252123f, 590787361f, 566599676f };
            byte[] singlesBytes = serializer.SerializeToBytes(singles);
            Single[] d_singles = serializer.DeserializeFromBytes<Single[]>(singlesBytes);
            Assert.IsTrue(singles.SequenceEqual(d_singles));

            Double[] doubles = { 6256504874604529222, 4102216363284542778, 5078741612481181031, 3690637210552029677, 5152917209682737889, 8904917999810835045, 3121983541514622568 };
            byte[] doublesBytes = serializer.SerializeToBytes(doubles);
            Double[] d_doubles = serializer.DeserializeFromBytes<Double[]>(doublesBytes);
            Assert.IsTrue(doubles.SequenceEqual(d_doubles));

            Char[] chars = { 'C', 'B', 'J', 'V' };
            byte[] charsBytes = serializer.SerializeToBytes(chars);
            Char[] d_chars = serializer.DeserializeFromBytes<Char[]>(charsBytes);
            Assert.IsTrue(chars.SequenceEqual(d_chars));

            String[] strings = { "", "Hello World!", "中文测试", "中文测试Hello World!Hello World!Hello World!中文测试" };
            byte[] stringsBytes = serializer.SerializeToBytes(strings);
            String[] d_strings = serializer.DeserializeFromBytes<String[]>(stringsBytes);
            Assert.IsTrue(strings.SequenceEqual(d_strings));
            #endregion

            #region 常规引用类型测试
            Assert.ThrowsException<ArgumentNullException>(() => serializer.SerializeToBytes(null!));
            Assert.ThrowsException<NullReferenceException>(() => serializer.SerializeToBytes(new Foo() { _strA = null! }));
            for (int i = 0; i < 100; i++)
            {
                Foo foo = Foo.GetRandomOne();

                for (int _ = 0; _ < 100; _++)
                {
                    byte[] fooBytes = serializer.SerializeToBytes(foo);
                    Foo d_Foo = serializer.DeserializeFromBytes<Foo>(fooBytes);
                    Assert.IsTrue(foo.Equals(d_Foo));
                }

            }

            Foo[] foos = new Foo[10];
            for (int i = 0; i < foos.Length; i++)
            {
                foos[i] = Foo.GetRandomOne();
            }
            byte[] foosBytes = serializer.SerializeToBytes(foos);
            Foo[] d_foos = serializer.DeserializeFromBytes<Foo[]>(foosBytes);
            Assert.AreEqual(foos.Length, d_foos.Length);
            for (int i = 0; i < 10; i++)
            {
                Assert.IsTrue(foos[i].Equals(d_foos[i]));
            }
            #endregion

            #region 复杂引用类型测试
            for (int i = 0; i < 100; i++)
            {
                ComplexFoo complexFoo = ComplexFoo.GetRandomOne();
                byte[] complexFooBytes = serializer.SerializeToBytes(complexFoo);
                ComplexFoo d_complexFoo = serializer.DeserializeFromBytes<ComplexFoo>(complexFooBytes);
                Assert.IsTrue(complexFoo.Equals(d_complexFoo));
            }

            ComplexFoo[] complexFoos = new ComplexFoo[10];
            for (int i = 0; i < complexFoos.Length; i++)
            {
                complexFoos[i] = ComplexFoo.GetRandomOne();
            }
            byte[] complexFoosBytes = serializer.SerializeToBytes(complexFoos);
            ComplexFoo[] d_complexFoos = serializer.DeserializeFromBytes<ComplexFoo[]>(complexFoosBytes);
            Assert.AreEqual(complexFoos.Length, d_complexFoos.Length);
            for (int i = 0; i < 10; i++)
            {
                Assert.IsTrue(complexFoos[i].Equals(d_complexFoos[i]));
            }
            #endregion

        }
    }
}
