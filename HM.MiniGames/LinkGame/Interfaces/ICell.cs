namespace HM.MiniGames.LinkGame
{
    public interface ICell
    {
        int ContentID { get; set; }
        CellState State { get; set; }
        Coordinate Coordinate { get; set; }
    }
}
