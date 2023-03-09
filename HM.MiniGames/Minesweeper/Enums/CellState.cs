namespace HM.MiniGames.Minesweeper.Enums
{
    public enum CellState
    {
        /// <summary>
        /// 未定义
        /// </summary>
        Undefined,
        /// <summary>
        /// 已打开
        /// </summary>
        Open,
        /// <summary>
        /// 未知
        /// </summary>
        Closed,
        /// <summary>
        /// 已标记
        /// </summary>
        Flagged,
        /// <summary>
        /// 错误标记的雷
        /// </summary>
        FalseFlagged,
        /// <summary>
        /// 点爆
        /// </summary>
        Blast
    }
}
