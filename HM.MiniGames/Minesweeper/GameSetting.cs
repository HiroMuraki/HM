namespace HM.MiniGames.Minesweeper
{
    public record class GameSetting : IGameSetting
    {
        public static readonly GameSetting None = new();

        public int Width { get; init; }
        public int Height { get; init; }
        public int MineCount { get; init; }
    }
}
