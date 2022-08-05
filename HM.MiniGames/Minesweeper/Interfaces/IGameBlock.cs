namespace HM.MiniGames.Minesweeper
{
    public interface IGameBlock
    {
        /// <summary>
        /// 游戏方块类型
        /// </summary>
        GameBlockType Type { get; set; }
        /// <summary>
        /// 游戏方块状态
        /// </summary>
        GameBlockState State { get; set; }
        /// <summary>
        /// 指示附近的雷数
        /// </summary>
        int NearbyMineCount { get; set; }
    }
}
