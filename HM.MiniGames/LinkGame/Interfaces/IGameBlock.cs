namespace HM.MiniGames.LinkGame
{
    public interface IGameBlock
    {
        int ContentID { get; set; }
        GamkeBlockState State { get; set; }
        Coordinate Coordinate { get; set; }
    }
}
