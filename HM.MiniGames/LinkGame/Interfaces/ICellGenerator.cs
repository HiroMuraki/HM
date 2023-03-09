namespace HM.MiniGames.LinkGame.Interfaces
{
    public interface ICellGenerator<TCell> where TCell : ICell
    {
        TCell CreateCell();
    }
}
