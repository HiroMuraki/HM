namespace HM.MiniGames.Minesweeper
{
    public enum GameCellOpenResult
    {
        /// <summary>
        /// 无效点击
        /// </summary>
        None,
        /// <summary>
        /// 正常打开
        /// </summary>
        Open,
        /// <summary>
        /// 点中雷块
        /// </summary>
        HitMine
    }
}
