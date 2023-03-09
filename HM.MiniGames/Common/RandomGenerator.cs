using System;

namespace HM.MiniGames.Common
{
    public class RandomGenerator : IRandomGenerator
    {
        public int Range(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }

        public RandomGenerator(int seed)
        {
            _random = new Random(seed);
        }
        public RandomGenerator()
        {
            _random = new Random();
        }

        private readonly Random _random;
    }
}
