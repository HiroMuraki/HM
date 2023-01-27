namespace HM.MiniGames.LinkGame
{
    public interface ICellGenerator<TCell> where TCell : ICell
    {
        TCell CreateCell();
    }
}
