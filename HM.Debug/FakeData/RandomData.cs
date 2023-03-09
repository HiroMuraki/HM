#pragma warning disable IDE0049

namespace HM.Debug.FakeData
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
        public static readonly char[] UpperLetters;
        public static readonly char[] LowerLetters;
        public static readonly char[] Digits;
        public static readonly char[] Symbols;

        public static byte NextByte() => NextByte(byte.MinValue, byte.MaxValue);
        public static byte NextByte(byte min = byte.MinValue, byte max = byte.MaxValue)
        {
            return (byte)Random.Next(min, max);
        }
        public static sbyte NextSByte() => NextSByte(sbyte.MinValue, sbyte.MaxValue);
        public static sbyte NextSByte(sbyte min = sbyte.MinValue, sbyte max = sbyte.MaxValue)
        {
            return (sbyte)Random.Next(min, max);
        }
        public static short NextInt16() => NextInt16(short.MinValue, short.MaxValue);
        public static short NextInt16(short min = short.MinValue, short max = short.MaxValue)
        {
            return (short)Random.Next(min, max);
        }
        public static ushort NextUInt16() => NextUInt16(ushort.MinValue, ushort.MaxValue);
        public static ushort NextUInt16(ushort min = ushort.MinValue, ushort max = ushort.MaxValue)
        {
            return (ushort)Random.Next(min, max);
        }
        public static int NextInt32() => NextInt32(int.MinValue, int.MaxValue);
        public static int NextInt32(int min = int.MinValue, int max = int.MaxValue)
        {
            return Random.Next(min, max);
        }
        public static uint NextUInt32() => NextUInt32(uint.MinValue, uint.MaxValue);
        public static uint NextUInt32(uint min = uint.MinValue, uint max = uint.MaxValue)
        {
            return (uint)Random.NextInt64(min, max);
        }
        public static long NextInt64() => NextInt64(long.MinValue, long.MaxValue);
        public static long NextInt64(long min = long.MinValue, long max = long.MaxValue)
        {
            return Random.NextInt64(min, max);
        }
        public static ulong NextUInt64() => NextUInt64(ulong.MinValue, ulong.MaxValue);
        public static ulong NextUInt64(ulong min = ulong.MinValue, ulong max = ulong.MaxValue)
        {
            return (ulong)Random.NextInt64((long)min, (long)max);
        }
        public static float NextSingle() => NextSingle(float.MinValue, float.MaxValue);
        public static float NextSingle(float min, float max)
        {
            return min + (float)Random.NextSingle() * (max - min)
                * Random.Next(0, 2) == 0 ? 1 : -1;
        }
        public static double NextDouble() => NextDouble(double.MinValue, double.MaxValue);
        public static double NextDouble(double min, double max)
        {
            return (min + (float)Random.NextDouble() * (max - min))
                * Random.Next(0, 2) == 0 ? 1 : -1;
        }
        public static decimal NextDecimal()
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
            int[] Int32Array =
            {
                Random.Next(int.MinValue, int.MaxValue),
                Random.Next(int.MinValue, int.MaxValue),
                Random.Next(int.MinValue, int.MaxValue),
                0
            };

            Int32Array[3] |= Random.Next(0, 29) << 16;
            if (Random.Next(0, 2) == 0)
            {
                Int32Array[3] |= unchecked((int)0b_10000000_00000000_00000000_00000000);
            }

            return new decimal(Int32Array);
        }
        public static bool NextBoolean()
        {
            return Random.Next(0, 2) == 0;
        }
        public static char NextChar(char min, char max)
        {
            return (char)Random.Next(min, max);
        }
        public static char NextChar() => NextChar(char.MinValue, char.MaxValue);
        public static DateTime NextDateTime() => NextDateTime(DateTime.MinValue, DateTime.MaxValue);
        public static DateTime NextDateTime(DateTime min, DateTime max)
        {
            return DateTime.FromBinary(Random.NextInt64(min.Ticks, max.Ticks));
        }
        public static string NextString(int minLength, int maxLength, CharTypes charTypes)
        {
            return NextString(Random.Next(minLength, maxLength + 1), charTypes);
        }
        public static string NextString(int minLength, int maxLength)
        {
            return NextString(Random.Next(minLength, maxLength + 1), CharTypes.All);
        }
        public static string NextString(int length, CharTypes charTypes)
        {
            var letters = new List<char>();
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

            return new string(ValuesFromPool(length, letters.ToArray()));
        }
        public static string NextString(int length)
        {
            return NextString(length, CharTypes.All);
        }
        public static string NextString(params CharTypes[] charTypes)
        {
            var result = new char[charTypes.Length];
            for (int i = 0; i < charTypes.Length; i++)
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
            return new string(result);
        }
        public static string NextString(string mask)
        {
            var charTypes = new CharTypes[mask.Length];
            for (int i = 0; i < charTypes.Length; i++)
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
        public static T[] Repeat<T>(Func<T> valueGetter, int count)
        {
            var result = new T[count];
            for (int i = 0; i < count; i++)
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
        public static T[] ValuesFromPool<T>(int size, params T[] values)
        {
            var result = new T[size];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = values[_random.Next(0, values.Length)];
            }
            return result;
        }

        private static Random _random = new();
        static RandomData()
        {
            UpperLetters = new char[26]
            {
                'A','B','C','D','E','F','G',
                'H','I','J','K','L','M','N',
                'O','P','Q','R','S','T','U',
                'V','W','X','Y','Z'
            };
            LowerLetters = new char[26]
            {
                'a','b','c','d','e','f','g',
                'h','i','j','k','l','m','n',
                'o','p','q','r','s','t','u',
                'v','w','x','y','z'
            };
            Digits = new char[10]
            {
                '0','1','2','3','4','5','6','7','8','9'
            };
            Symbols = new char[32]
            {
                '!','"','#','$','%','&','\'','(',
                ')','*','+',',','-','.','/',':',
                ';','<','=','>','?','@','[','\\',
                ']','^','_','`','{','|','}','~',
            };
        }
    }
}
