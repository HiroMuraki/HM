using HM.MiniGames.Common;
using HM.MiniGames.LinkGame.Enums;

namespace HM.MiniGames.LinkGame.Interfaces
{
    public interface ICell
    {
        int ContentID { get; set; }
        CellState State { get; set; }
        Coordinate Coordinate { get; set; }
    }
}
