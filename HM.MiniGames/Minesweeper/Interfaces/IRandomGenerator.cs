namespace HM.MiniGames.Minesweeper
{
    public interface IRandomGenerator
    {
        /// <summary>
        /// Generate a random value between [minValue, maxValue)
        /// </summary>
        /// <param name="minValue">min value(inclusive)</param>
        /// <param name="maxValue">max value(exclusive)</param>
        /// <returns>random value between [minValue, maxValue)</returns>
        int Range(int minValue, int maxValue);
    }
}
