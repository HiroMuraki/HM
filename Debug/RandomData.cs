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
        public static string RandomString(int minLength, int maxLength, CharTypes charTypes)
        {
            return RandomString(new Random().Next(minLength, maxLength + 1), charTypes);
        }
        /// <summary>
        /// Generate a random string with specific length
        /// </summary>
        /// <param name="length"></param>
        /// <param name="charTypes"></param>
        /// <returns></returns>
        public static string RandomString(int length, CharTypes charTypes)
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

            return new string(RandomArray(length, letters.ToArray()));
        }
        /// <summary>
        /// Generate a random string from char type array
        /// </summary>
        /// <param name="charTypes"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string RandomString(params CharTypes[] charTypes)
        {
            var rnd = new Random();
            var result = new char[charTypes.Length];
            for (int i = 0; i < charTypes.Length; i++)
            {
                result[i] = charTypes[i] switch
                {
                    CharTypes.UpperLetters => RandomValue(rnd, UpperLetters),
                    CharTypes.LowerLetters => RandomValue(rnd, LowerLetters),
                    CharTypes.Digits => RandomValue(rnd, Digits),
                    CharTypes.Symbols => RandomValue(rnd, Symbols),
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
        public static string RandomString(string mask)
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
            return RandomString(charTypes);
        }
        /// <summary>
        /// Generate a random string with specific length, chars from char pools
        /// </summary>
        /// <param name="length"></param>
        /// <param name="charPools"></param>
        /// <returns></returns>
        public static string RandomString(int length, params char[][] charPools)
        {
            return new string(RandomArray(length, charPools));
        }
        /// <summary>
        /// Generate a random string with specific length, sub strings from char pools
        /// </summary>
        /// <param name="length"></param>
        /// <param name="charPools"></param>
        /// <returns></returns>
        public static string RandomString(int length, params string[][] charPools)
        {
            var sb = new StringBuilder();
            foreach (var item in RandomArray(length, charPools))
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
        public static string RandomString(int minLength, int maxLength, params char[][] charPools)
        {
            return new string(RandomArray(new Random().Next(minLength, maxLength + 1), charPools));
        }
        /// <summary>
        /// Generate a random string with in range, sub strings from char pools
        /// </summary>
        /// <param name="length"></param>
        /// <param name="charPools"></param>
        /// <returns></returns>
        public static string RandomString(int minLength, int maxLength, params string[][] charPools)
        {
            var sb = new StringBuilder();
            foreach (var item in RandomArray(new Random().Next(minLength, maxLength + 1), charPools))
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
        public static int[] RandomInts(int length, int min, int max)
        {
            if (max < min)
            {
                throw new ArgumentException("Max should not be less than min");
            }
            return RandomArray(length, Enumerable.Range(min, max - min + 1).ToArray());
        }
        /// <summary>
        /// Randomly pick a value from value pool
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valuePool"></param>
        /// <returns></returns>
        public static T RandomValue<T>(params T[] valuePool)
        {
            return RandomValue(new Random(), valuePool);
        }
        /// <summary>
        /// Randomly pick a value from value pools
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valuePools"></param>
        /// <returns></returns>
        public static T RandomValue<T>(params T[][] valuePools)
        {
            return RandomValue(new Random(), valuePools);
        }
        /// <summary>
        /// Random pick up specific count of values from value pools
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="length"></param>
        /// <param name="valuePools"></param>
        /// <returns></returns>
        public static T[] RandomArray<T>(int length, params T[][] valuePools)
        {
            return RandomArray(length, new Random(), valuePools);
        }
        /// <summary>
        /// Random pick up specific count of values from value pools with specific random seed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="length"></param>
        /// <param name="valuePools"></param>
        /// <returns></returns>
        public static T[] RandomArray<T>(int length, int rndSeed, params T[][] valuePools)
        {
            return RandomArray(length, new Random(rndSeed), valuePools);
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
        private static T RandomValue<T>(Random rnd, params T[] valuePool)
        {
            return valuePool[rnd.Next(0, valuePool.Length)];
        }
        private static T RandomValue<T>(Random rnd, params T[][] valuePools)
        {
            var pool = valuePools[rnd.Next(0, valuePools.Length)];
            return pool[rnd.Next(0, pool.Length)];
        }
        private static T[] RandomArray<T>(int length, Random rnd, params T[][] valuePools)
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
