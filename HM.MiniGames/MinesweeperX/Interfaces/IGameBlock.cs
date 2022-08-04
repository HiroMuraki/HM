namespace HM.MiniGames.MinesweeperX
{
    public interface IGameBlock
    {
        /// <summary>
        /// 游戏方块类型
        /// </summary>
        GameBlockType BlockType { get; set; }
        /// <summary>
        /// 游戏方块状态
        /// </summary>
        GameBlockStatus BlockStatus { get; set; }
        /// <summary>
        /// 指示附近的雷数
        /// </summary>
        int NearbyMineCount { get; set; }
    }
}
