namespace HM.MiniGames.Minesweeper
{
    public interface IGameCellGenerator<TOutCell> where TOutCell: IGameCell
    {
        /// <summary>
        /// 生成一个GameCell
        /// </summary>
        /// <returns></returns>
        TOutCell GetGameCell();
    }
}
