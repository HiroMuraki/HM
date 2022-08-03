namespace HM.MiniGames.MinesweeperX
{
    public interface IGameSetting
    {
        public int Width { get; }
        public int Height { get; }
        public int MineCount { get; }
    }
}
