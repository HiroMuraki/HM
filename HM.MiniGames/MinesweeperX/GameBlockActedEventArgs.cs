namespace HM.MiniGames.MinesweeperX
{
    public class GameBlockActedEventArgs : EventArgs
    {
        public IGameBlock? GameBlock { get; init; }
        public GameBlockAction Action { get; init; }
    }
}
