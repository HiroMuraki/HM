using HM.MiniGames.Minesweeper.Enums;

namespace HM.MiniGames.Minesweeper.Interfaces
{
    public interface ICell
    {
        /// <summary>
        /// 游戏方块类型
        /// </summary>
        CellType Type { get; set; }
        /// <summary>
        /// 游戏方块状态
        /// </summary>
        CellState State { get; set; }
        /// <summary>
        /// 指示附近的雷数
        /// </summary>
        int NearbyMineCount { get; set; }
    }
}
