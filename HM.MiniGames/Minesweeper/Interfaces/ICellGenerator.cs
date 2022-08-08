namespace HM.MiniGames.Minesweeper
{
    public interface ICellGenerator
    {
        /// <summary>
        /// 生成一个GameBlock
        /// </summary>
        /// <returns></returns>
        ICell GetCell();
    }
}
