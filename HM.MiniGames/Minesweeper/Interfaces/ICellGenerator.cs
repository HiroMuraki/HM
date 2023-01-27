namespace HM.MiniGames.Minesweeper
{
    public interface ICellGenerator<TCell> where TCell: ICell
    {
        /// <summary>
        /// 生成一个GameCell
        /// </summary>
        /// <returns></returns>
        TCell CreateCell();
    }
}
