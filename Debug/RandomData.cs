#pragma warning disable IDE0049
using System.Text;

namespace HM.Debug
{
    public static class RandomData
    {
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
            var rnd = new Random();
            var result = new char[charTypes.Length];
            for (int i = 0; i < charTypes.Length; i++)
            {
                result[i] = charTypes[i] switch
                {
                    CharTypes.UpperLetters => NextValue(rnd, UpperLetters),
                    CharTypes.LowerLetters => NextValue(rnd, LowerLetters),
                    CharTypes.Digits => NextValue(rnd, Digits),
                    CharTypes.Symbols => NextValue(rnd, Symbols),
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
        /// <param name="charPools"></param>
        /// <returns></returns>
        public static string NextString(int length, params string[][] charPools)
        {
            var sb = new StringBuilder();
            foreach (var item in NextArray(length, charPools))
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
        /// <summary>
        /// Generate a array of random int value in range
        /// </summary>
        /// <param name="length"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static int[] NextInts(int length, int min, int max)
        {
            if (max < min)
            {
                throw new ArgumentException("Max should not be less than min");
            }
            return NextArray(length, Enumerable.Range(min, max - min + 1).ToArray());
        }
        /// <summary>
        /// Nextly pick a value from value pool
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valuePool"></param>
        /// <returns></returns>
        public static T NextValue<T>(params T[] valuePool)
        {
            return NextValue(new Random(), valuePool);
        }
        /// <summary>
        /// Nextly pick a value from value pools
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valuePools"></param>
        /// <returns></returns>
        public static T NextValue<T>(params T[][] valuePools)
        {
            return NextValue(new Random(), valuePools);
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
            return NextArray(length, new Random(), valuePools);
        }
        /// <summary>
        /// Next pick up specific count of values from value pools with specific random seed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="length"></param>
        /// <param name="valuePools"></param>
        /// <returns></returns>
        public static T[] NextArray<T>(int length, int rndSeed, params T[][] valuePools)
        {
            return NextArray(length, new Random(rndSeed), valuePools);
        }

        public static Byte NextByte()
        {
            var rnd = new Random();
            return (Byte)rnd.Next(Byte.MinValue, Byte.MaxValue);
        }
        public static SByte NextSByte()
        {
            var rnd = new Random();
            return (SByte)rnd.Next(SByte.MinValue, SByte.MaxValue);
        }
        public static Int16 NextInt16()
        {
            var rnd = new Random();
            return (Int16)rnd.Next(Int16.MinValue, Int16.MaxValue);
        }
        public static UInt16 NextUInt16()
        {
            var rnd = new Random();
            return (UInt16)rnd.Next(UInt16.MinValue, UInt16.MaxValue);
        }
        public static Int32 NextInt32()
        {
            var rnd = new Random();
            return (Int32)rnd.Next(Int32.MinValue, Int32.MaxValue);
        }
        public static UInt32 NextUInt32()
        {
            var rnd = new Random();
            return (UInt32)rnd.Next(Int32.MinValue, Int32.MaxValue);
        }
        public static Int64 NextInt64()
        {
            var rnd = new Random();
            return (Int64)rnd.NextInt64(Int64.MinValue, Int64.MaxValue);
        }
        public static UInt64 NextUInt64()
        {
            var rnd = new Random();
            return (UInt64)rnd.NextInt64(Int64.MinValue, Int64.MaxValue);
        }
        public static Single NextSingle()
        {
            var rnd = new Random();
            return (Single)rnd.NextSingle() * Single.MaxValue * (rnd.Next(0, 2) == 0 ? 1 : -1);
        }
        public static Double NextDouble()
        {
            var rnd = new Random();
            return (Double)rnd.NextDouble() * Double.MaxValue * (rnd.Next(0, 2) == 0 ? 1 : -1);
        }
        public static Boolean NextBoolean()
        {
            var rnd = new Random();
            return new Random().Next(0, 2) == 0;
        }
        public static Char NextChar()
        {
            var rnd = new Random();
            return (Char)rnd.Next(Char.MinValue, Char.MaxValue);
        }
        public static String NextString()
        {
            return NextString(0, 255, CharTypes.All);
        }

        public static Byte[] NextBytes(int maxSize = 100)
        {
            var rnd = new Random();
            Byte[] result = new Byte[rnd.Next(0, maxSize)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = NextByte();
            }
            return result;
        }
        public static SByte[] NextSBytes(int maxSize = 100)
        {
            var rnd = new Random();
            SByte[] result = new SByte[rnd.Next(0, maxSize)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = NextSByte();
            }
            return result;
        }
        public static Int16[] NextInt16s(int maxSize = 100)
        {
            var rnd = new Random();
            Int16[] result = new Int16[rnd.Next(0, maxSize)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = NextInt16();
            }
            return result;
        }
        public static UInt16[] NextUInt16s(int maxSize = 100)
        {
            var rnd = new Random();
            UInt16[] result = new UInt16[rnd.Next(0, maxSize)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = NextUInt16();
            }
            return result;
        }
        public static Int32[] NextInt32s(int maxSize = 100)
        {
            var rnd = new Random();
            Int32[] result = new Int32[rnd.Next(0, maxSize)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = NextInt32();
            }
            return result;
        }
        public static UInt32[] NextUInt32s(int maxSize = 100)
        {
            var rnd = new Random();
            UInt32[] result = new UInt32[rnd.Next(0, maxSize)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = NextUInt32();
            }
            return result;
        }
        public static Int64[] NextInt64s(int maxSize = 100)
        {
            var rnd = new Random();
            Int64[] result = new Int64[rnd.Next(0, maxSize)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = NextInt64();
            }
            return result;
        }
        public static UInt64[] NextUInt64s(int maxSize = 100)
        {
            var rnd = new Random();
            UInt64[] result = new UInt64[rnd.Next(0, maxSize)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = NextUInt64();
            }
            return result;
        }
        public static Single[] NextSingles(int maxSize = 100)
        {
            var rnd = new Random();
            Single[] result = new Single[rnd.Next(0, maxSize)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = NextSingle();
            }
            return result;
        }
        public static Double[] NextDoubles(int maxSize = 100)
        {
            var rnd = new Random();
            Double[] result = new Double[rnd.Next(0, maxSize)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = NextDouble();
            }
            return result;
        }
        public static Boolean[] NextBooleans(int maxSize = 100)
        {
            var rnd = new Random();
            Boolean[] result = new Boolean[rnd.Next(0, maxSize)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = NextBoolean();
            }
            return result;
        }
        public static Char[] NextChars(int maxSize = 100)
        {
            var rnd = new Random();
            Char[] result = new Char[rnd.Next(0, maxSize)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = NextChar();
            }
            return result;
        }
        public static String[] NextStrings(int maxSize = 100)
        {
            var rnd = new Random();
            String[] result = new String[rnd.Next(0, maxSize)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = NextString();
            }
            return result;
        }
        public static T[] NextArray<T>(Func<T> creater, int maxSize = 100)
        {
            ArgumentNullException.ThrowIfNull(creater);

            var rnd = new Random();
            T[] result = new T[rnd.Next(0, maxSize)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] =creater();
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
        private static T NextValue<T>(Random rnd, params T[] valuePool)
        {
            return valuePool[rnd.Next(0, valuePool.Length)];
        }
        private static T NextValue<T>(Random rnd, params T[][] valuePools)
        {
            var pool = valuePools[rnd.Next(0, valuePools.Length)];
            return pool[rnd.Next(0, pool.Length)];
        }
        private static T[] NextArray<T>(int length, Random rnd, params T[][] valuePools)
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
                int poolID = rnd.Next(0, valuePools.Length);
                result[i] = valuePools[poolID][rnd.Next(0, valuePools[poolID].Length)];
            }
            return result;
        }
    }
}
