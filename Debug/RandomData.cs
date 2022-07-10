#pragma warning disable IDE0049
using System.Text;

namespace HM.Debug
{
    public static class RandomData
    {
        public static Random Random { get; } = new Random();
        public static readonly char[] UpperLetters;
        public static readonly char[] LowerLetters;
        public static readonly char[] Digits;
        public static readonly char[] Symbols;

        /// <summary>
        /// Generate a random string with random length in range
        /// </summary>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        /// <param name="charTypes"></param>
        /// <returns></returns>
        public static string NextString(int minLength, int maxLength, CharTypes charTypes)
        {
            return NextString(new Random().Next(minLength, maxLength + 1), charTypes);
        }
        /// <summary>
        /// Generate a random string with specific length
        /// </summary>
        /// <param name="length"></param>
        /// <param name="charTypes"></param>
        /// <returns></returns>
        public static string NextString(int length, CharTypes charTypes)
        {
            var letters = new List<char[]>();
            if ((charTypes & CharTypes.UpperLetters) == CharTypes.UpperLetters)
            {
                letters.Add(UpperLetters);
            }
            if ((charTypes & CharTypes.LowerLetters) == CharTypes.LowerLetters)
            {
                letters.Add(LowerLetters);
            }
            if ((charTypes & CharTypes.Digits) == CharTypes.Digits)
            {
                letters.Add(Digits);
            }
            if ((charTypes & CharTypes.Symbols) == CharTypes.Symbols)
            {
                letters.Add(Symbols);
            }

            return new string(NextArray(length, letters.ToArray()));
        }
        /// <summary>
        /// Generate a random string from char type array
        /// </summary>
        /// <param name="charTypes"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string NextString(params CharTypes[] charTypes)
        {
            var result = new char[charTypes.Length];
            for (int i = 0; i < charTypes.Length; i++)
            {
                result[i] = charTypes[i] switch
                {
                    CharTypes.UpperLetters => NextValue(UpperLetters),
                    CharTypes.LowerLetters => NextValue(LowerLetters),
                    CharTypes.Digits => NextValue(Digits),
                    CharTypes.Symbols => NextValue(Symbols),
                    _ => throw new ArgumentException($"Unsupported char type {charTypes[i]}")
                };
            }
            return new string(result);
        }
        /// <summary>
        /// Generate a random string from mask
        /// </summary>
        /// <param name="mask">`A`: upper letters 'a': lower letters, '0': digits, '^': symbols</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Raise if any unsuppored mask char included</exception>
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
                    _ => throw new ArgumentException($"Invalid char mask `{mask[i]}`")
                };
            }
            return NextString(charTypes);
        }
        /// <summary>
        /// Generate a random string with specific length, chars from char pools
        /// </summary>
        /// <param name="length"></param>
        /// <param name="charPools"></param>
        /// <returns></returns>
        public static string NextString(int length, params char[][] charPools)
        {
            return new string(NextArray(length, charPools));
        }
        /// <summary>
        /// Generate a random string with specific length, sub strings from char pools
        /// </summary>
        /// <param name="length"></param>
        /// <param name="stringPools"></param>
        /// <returns></returns>
        public static string NextString(int length, params string[][] stringPools)
        {
            var sb = new StringBuilder();
            foreach (var item in NextArray(length, stringPools))
            {
                sb.Append(item);
            }
            return sb.ToString();
        }
        /// <summary>
        /// Generate a random string with in range, chars from char pools
        /// </summary>
        /// <param name="length"></param>
        /// <param name="charPools"></param>
        /// <returns></returns>
        public static string NextString(int minLength, int maxLength, params char[][] charPools)
        {
            return new string(NextArray(new Random().Next(minLength, maxLength + 1), charPools));
        }
        /// <summary>
        /// Generate a random string with in range, sub strings from char pools
        /// </summary>
        /// <param name="length"></param>
        /// <param name="charPools"></param>
        /// <returns></returns>
        public static string NextString(int minLength, int maxLength, params string[][] charPools)
        {
            var sb = new StringBuilder();
            foreach (var item in NextArray(new Random().Next(minLength, maxLength + 1), charPools))
            {
                sb.Append(item);
            }
            return sb.ToString();
        }

        public static Byte NextByte()
        {
            return (Byte)Random.Next(Byte.MinValue, Byte.MaxValue);
        }
        public static SByte NextSByte()
        {
            return (SByte)Random.Next(SByte.MinValue, SByte.MaxValue);
        }
        public static Int16 NextInt16()
        {
            return (Int16)Random.Next(Int16.MinValue, Int16.MaxValue);
        }
        public static UInt16 NextUInt16()
        {
            return (UInt16)Random.Next(UInt16.MinValue, UInt16.MaxValue);
        }
        public static Int32 NextInt32()
        {
            return (Int32)Random.Next(Int32.MinValue, Int32.MaxValue);
        }
        public static UInt32 NextUInt32()
        {
            return (UInt32)Random.Next(Int32.MinValue, Int32.MaxValue);
        }
        public static Int64 NextInt64()
        {
            return (Int64)Random.NextInt64(Int64.MinValue, Int64.MaxValue);
        }
        public static UInt64 NextUInt64()
        {
            return (UInt64)Random.NextInt64(Int64.MinValue, Int64.MaxValue);
        }
        public static Single NextSingle()
        {
            return (Single)Random.NextSingle() * Single.MaxValue * (Random.Next(0, 2) == 0 ? 1 : -1);
        }
        public static Double NextDouble()
        {
            return (Double)Random.NextDouble() * Double.MaxValue * (Random.Next(0, 2) == 0 ? 1 : -1);
        }
        public static Boolean NextBoolean()
        {
            return new Random().Next(0, 2) == 0;
        }
        public static Char NextChar()
        {
            return (Char)Random.Next(Char.MinValue, Char.MaxValue);
        }
        public static DateTime NextDateTime()
        {
            return DateTime.FromBinary(Random.NextInt64(DateTime.MinValue.Ticks, DateTime.MaxValue.Ticks));
        }
        public static String NextString()
        {
            return NextString(0, 255, CharTypes.All);
        }

        public static Byte[] NextBytes(int maxSize = 100)
        {
            Byte[] result = new Byte[Random.Next(0, maxSize)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = NextByte();
            }
            return result;
        }
        public static SByte[] NextSBytes(int maxSize = 100)
        {
            SByte[] result = new SByte[Random.Next(0, maxSize)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = NextSByte();
            }
            return result;
        }
        public static Int16[] NextInt16s(int maxSize = 100)
        {
            Int16[] result = new Int16[Random.Next(0, maxSize)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = NextInt16();
            }
            return result;
        }
        public static UInt16[] NextUInt16s(int maxSize = 100)
        {
            UInt16[] result = new UInt16[Random.Next(0, maxSize)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = NextUInt16();
            }
            return result;
        }
        public static Int32[] NextInt32s(int maxSize = 100)
        {
            Int32[] result = new Int32[Random.Next(0, maxSize)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = NextInt32();
            }
            return result;
        }
        public static UInt32[] NextUInt32s(int maxSize = 100)
        {
            UInt32[] result = new UInt32[Random.Next(0, maxSize)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = NextUInt32();
            }
            return result;
        }
        public static Int64[] NextInt64s(int maxSize = 100)
        {
            Int64[] result = new Int64[Random.Next(0, maxSize)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = NextInt64();
            }
            return result;
        }
        public static UInt64[] NextUInt64s(int maxSize = 100)
        {
            UInt64[] result = new UInt64[Random.Next(0, maxSize)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = NextUInt64();
            }
            return result;
        }
        public static Single[] NextSingles(int maxSize = 100)
        {
            Single[] result = new Single[Random.Next(0, maxSize)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = NextSingle();
            }
            return result;
        }
        public static Double[] NextDoubles(int maxSize = 100)
        {
            Double[] result = new Double[Random.Next(0, maxSize)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = NextDouble();
            }
            return result;
        }
        public static Boolean[] NextBooleans(int maxSize = 100)
        {
            Boolean[] result = new Boolean[Random.Next(0, maxSize)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = NextBoolean();
            }
            return result;
        }
        public static Char[] NextChars(int maxSize = 100)
        {
            Char[] result = new Char[Random.Next(0, maxSize)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = NextChar();
            }
            return result;
        }
        public static DateTime[] NextDateTimes(int maxSize = 100)
        {
            DateTime[] result = new DateTime[Random.Next(0, maxSize)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = NextDateTime();
            }
            return result;
        }
        public static String[] NextStrings(int maxSize = 100)
        {
            String[] result = new String[Random.Next(0, maxSize)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = NextString();
            }
            return result;
        }
        /// <summary>
        /// Nextly pick a value from value pool
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valuePool"></param>
        /// <returns></returns>
        public static T NextValue<T>(params T[] valuePool)
        {
            return valuePool[Random.Next(0, valuePool.Length)];
        }
        /// <summary>
        /// Nextly pick a value from value pools
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valuePools"></param>
        /// <returns></returns>
        public static T NextValue<T>(params T[][] valuePools)
        {
            var pool = valuePools[Random.Next(0, valuePools.Length)];
            return pool[Random.Next(0, pool.Length)];
        }
        public static T[] NextArray<T>(Func<T> creater, int maxSize = 100)
        {
            ArgumentNullException.ThrowIfNull(creater);

            T[] result = new T[Random.Next(0, maxSize)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = creater();
            }
            return result;
        }
        /// <summary>
        /// Next pick up specific count of values from value pools
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="length"></param>
        /// <param name="valuePools"></param>
        /// <returns></returns>
        public static T[] NextArray<T>(int length, params T[][] valuePools)
        {
            if (valuePools.Length == 0)
            {
                throw new ArgumentException("Require at least one value pool");
            }
            foreach (var pool in valuePools)
            {
                if (pool.Length == 0)
                {
                    throw new ArgumentException("Empty pool is not allowed");
                }
            }
            var result = new T[length];
            for (int i = 0; i < length; i++)
            {
                int poolID = Random.Next(0, valuePools.Length);
                result[i] = valuePools[poolID][Random.Next(0, valuePools[poolID].Length)];
            }
            return result;
        }

        static RandomData()
        {
            UpperLetters = new char[26] {
                'A','B','C','D','E','F','G',
                'H','I','J','K','L','M','N',
                'O','P','Q','R','S','T','U',
                'V','W','X','Y','Z'
            };
            LowerLetters = new char[26] {
                'a','b','c','d','e','f','g',
                'h','i','j','k','l','m','n',
                'o','p','q','r','s','t','u',
                'v','w','x','y','z'
            };
            Digits = new char[10] {
                '0','1','2','3','4','5','6','7','8','9'
            };
            Symbols = new char[32] {
                '!','"','#','$','%','&','\'','(',
                ')','*','+',',','-','.','/',':',
                ';','<','=','>','?','@','[','\\',
                ']','^','_','`','{','|','}','~',
            };
        }
    }
}
