namespace HM.MiniGames.MinesweeperX
{
    public interface IGameBlock
    {
        GameBlockType BlockType { get; set; }
        GameBlockStatus BlockStatus { get; set; }
        int NearbyMineCount { get; set; }
    }
}
