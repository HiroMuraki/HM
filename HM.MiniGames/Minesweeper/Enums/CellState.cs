namespace HM.MiniGames.Minesweeper
{
    public enum CellState
    {
        /// <summary>
        /// 未定义
        /// </summary>
        Undefined,
        /// <summary>
        /// 已揭示
        /// </summary>
        Proclaimed,
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
    }
}
