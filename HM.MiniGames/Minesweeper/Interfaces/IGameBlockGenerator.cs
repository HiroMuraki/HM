namespace HM.MiniGames.Minesweeper
{
    public interface IGameBlockGenerator
    {
        /// <summary>
        /// 生成一个GameBlock
        /// </summary>
        /// <returns></returns>
        IGameBlock GetGameBlock();
    }
}
