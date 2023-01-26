namespace HM.MiniGames.Minesweeper
{
    public interface IGameCell
    {
        /// <summary>
        /// 游戏方块类型
        /// </summary>
        GameCellType Type { get; set; }
        /// <summary>
        /// 游戏方块状态
        /// </summary>
        GameCellState State { get; set; }
        /// <summary>
        /// 指示附近的雷数
        /// </summary>
        int NearbyMineCount { get; set; }
    }
}
