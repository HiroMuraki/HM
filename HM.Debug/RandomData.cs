#pragma warning disable IDE0049
using System.Text;

namespace HM.Debug
{
    public static class RandomData
    {
        public static Random Random
        {
            get => _random;
            set
            {
                ArgumentNullException.ThrowIfNull(value);
                _random = value;
            }
        }
        public static readonly Char[] UpperLetters;
        public static readonly Char[] LowerLetters;
        public static readonly Char[] Digits;
        public static readonly Char[] Symbols;

        public static Byte NextByte() => NextByte(Byte.MinValue, Byte.MaxValue);
        public static Byte NextByte(Byte min = Byte.MinValue, Byte max = Byte.MaxValue)
        {
            return (Byte)Random.Next(min, max);
        }
        public static SByte NextSByte() => NextSByte(SByte.MinValue, SByte.MaxValue);
        public static SByte NextSByte(SByte min = SByte.MinValue, SByte max = SByte.MaxValue)
        {
            return (SByte)Random.Next(min, max);
        }
        public static Int16 NextInt16() => NextInt16(Int16.MinValue, Int16.MaxValue);
        public static Int16 NextInt16(Int16 min = Int16.MinValue, Int16 max = Int16.MaxValue)
        {
            return (Int16)Random.Next(min, max);
        }
        public static UInt16 NextUInt16() => NextUInt16(UInt16.MinValue, UInt16.MaxValue);
        public static UInt16 NextUInt16(UInt16 min = UInt16.MinValue, UInt16 max = UInt16.MaxValue)
        {
            return (UInt16)Random.Next(min, max);
        }
        public static Int32 NextInt32() => NextInt32(Int32.MinValue, Int32.MaxValue);
        public static Int32 NextInt32(Int32 min = Int32.MinValue, Int32 max = Int32.MaxValue)
        {
            return (Int32)Random.Next(min, max);
        }
        public static UInt32 NextUInt32() => NextUInt32(UInt32.MinValue, UInt32.MaxValue);
        public static UInt32 NextUInt32(UInt32 min = UInt32.MinValue, UInt32 max = UInt32.MaxValue)
        {
            return (UInt32)Random.NextInt64(min, max);
        }
        public static Int64 NextInt64() => NextInt64(Int64.MinValue, Int64.MaxValue);
        public static Int64 NextInt64(Int64 min = Int64.MinValue, Int64 max = Int64.MaxValue)
        {
            return (Int64)Random.NextInt64(min, max);
        }
        public static UInt64 NextUInt64() => NextUInt64(UInt64.MinValue, UInt64.MaxValue);
        public static UInt64 NextUInt64(UInt64 min = UInt64.MinValue, UInt64 max = UInt64.MaxValue)
        {
            return (UInt64)Random.NextInt64((Int64)min, (Int64)max);
        }
        public static Single NextSingle() => NextSingle(Single.MinValue, Single.MaxValue);
        public static Single NextSingle(Single min, Single max)
        {
            return min + (Single)Random.NextSingle() * (max - min)
                * Random.Next(0, 2) == 0 ? 1 : -1;
        }
        public static Double NextDouble() => NextDouble(Double.MinValue, Double.MaxValue);
        public static Double NextDouble(Double min, Double max)
        {
            return (min + (Single)Random.NextDouble() * (max - min))
                * Random.Next(0, 2) == 0 ? 1 : -1;
        }
        public static Decimal NextDecimal()
        {
            // [前96比特不用管，用于表示[0,2^96-1]的数，任意取值即可]
            // 下面是标志位[Int32]的结构
            // [00000000 00000000 00000000 00000000]
            // [                  ffffffff        s] x
            // [s        ffffffff                  ]
            // [00000000 000fffff 00000000 0000000s]
            // s = 符号位，0正1负
            // f = 比例因子，包含一个[0,28]的整数，指示10的幂（即小数从右往左的偏移位）,
            //     由于实际上只需要5个bit即可表示[0,28]，而剩下的三位一定是0
            // 0 = 常量0
            Int32[] Int32Array =
            {
                Random.Next(Int32.MinValue, Int32.MaxValue),
                Random.Next(Int32.MinValue, Int32.MaxValue),
                Random.Next(Int32.MinValue, Int32.MaxValue),
                0
            };

            Int32Array[3] |= Random.Next(0, 29) << 16;
            if (Random.Next(0, 2) == 0)
            {
                Int32Array[3] |= unchecked((Int32)0b_10000000_00000000_00000000_00000000);
            }

            return new decimal(Int32Array);
        }
        public static Boolean NextBoolean()
        {
            return Random.Next(0, 2) == 0;
        }
        public static Char NextChar(Char min, Char max)
        {
            return (Char)Random.Next(min, max);
        }
        public static Char NextChar() => NextChar(Char.MinValue, Char.MaxValue);
        public static DateTime NextDateTime() => NextDateTime(DateTime.MinValue, DateTime.MaxValue);
        public static DateTime NextDateTime(DateTime min, DateTime max)
        {
            return DateTime.FromBinary(Random.NextInt64(min.Ticks, max.Ticks));
        }
        public static String NextString(Int32 minLength, Int32 maxLength, CharTypes charTypes)
        {
            return NextString(Random.Next(minLength, maxLength + 1), charTypes);
        }
        public static String NextString(Int32 minLength, Int32 maxLength)
        {
            return NextString(Random.Next(minLength, maxLength + 1), CharTypes.All);
        }
        public static String NextString(Int32 length, CharTypes charTypes)
        {
            var letters = new List<Char>();
            if ((charTypes & CharTypes.UpperLetters) == CharTypes.UpperLetters)
            {
                letters.AddRange(UpperLetters);
            }
            if ((charTypes & CharTypes.LowerLetters) == CharTypes.LowerLetters)
            {
                letters.AddRange(LowerLetters);
            }
            if ((charTypes & CharTypes.Digits) == CharTypes.Digits)
            {
                letters.AddRange(Digits);
            }
            if ((charTypes & CharTypes.Symbols) == CharTypes.Symbols)
            {
                letters.AddRange(Symbols);
            }

            return new String(ValuesFromPool(length, letters.ToArray()));
        }
        public static String NextString(Int32 length)
        {
            return NextString(length, CharTypes.All);
        }
        public static String NextString(params CharTypes[] charTypes)
        {
            var result = new Char[charTypes.Length];
            for (Int32 i = 0; i < charTypes.Length; i++)
            {
                result[i] = charTypes[i] switch
                {
                    CharTypes.UpperLetters => ValueFromPool(UpperLetters),
                    CharTypes.LowerLetters => ValueFromPool(LowerLetters),
                    CharTypes.Digits => ValueFromPool(Digits),
                    CharTypes.Symbols => ValueFromPool(Symbols),
                    _ => throw new ArgumentException($"Unsupported Char type {charTypes[i]}")
                };
            }
            return new String(result);
        }
        public static String NextString(String mask)
        {
            var charTypes = new CharTypes[mask.Length];
            for (Int32 i = 0; i < charTypes.Length; i++)
            {
                charTypes[i] = mask[i] switch
                {
                    'A' => CharTypes.UpperLetters,
                    'a' => CharTypes.LowerLetters,
                    '0' => CharTypes.Digits,
                    '^' => CharTypes.Symbols,
                    _ => throw new ArgumentException($"Invalid Char mask `{mask[i]}`")
                };
            }
            return NextString(charTypes);
        }

        /// <summary>
        /// Repeatly generate values by value getter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valueGetter"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static T[] Repeat<T>(Func<T> valueGetter, Int32 count)
        {
            var result = new T[count];
            for (Int32 i = 0; i < count; i++)
            {
                result[i] = valueGetter();
            }
            return result;
        }
        /// <summary>
        /// Randomly pick a value from value pool
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valuePool"></param>
        /// <returns></returns>
        public static T ValueFromPool<T>(params T[] valuePool)
        {
            return valuePool[Random.Next(0, valuePool.Length)];
        }
        /// <summary>
        /// Randomly pick values from value pool
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="size"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static T[] ValuesFromPool<T>(Int32 size, params T[] values)
        {
            var result = new T[size];
            for (Int32 i = 0; i < result.Length; i++)
            {
                result[i] = values[_random.Next(0, values.Length)];
            }
            return result;
        }

        private static Random _random = new();
        static RandomData()
        {
            UpperLetters = new Char[26]
            {
                'A','B','C','D','E','F','G',
                'H','I','J','K','L','M','N',
                'O','P','Q','R','S','T','U',
                'V','W','X','Y','Z'
            };
            LowerLetters = new Char[26]
            {
                'a','b','c','d','e','f','g',
                'h','i','j','k','l','m','n',
                'o','p','q','r','s','t','u',
                'v','w','x','y','z'
            };
            Digits = new Char[10]
            {
                '0','1','2','3','4','5','6','7','8','9'
            };
            Symbols = new Char[32]
            {
                '!','"','#','$','%','&','\'','(',
                ')','*','+',',','-','.','/',':',
                ';','<','=','>','?','@','[','\\',
                ']','^','_','`','{','|','}','~',
            };
        }
    }
}
