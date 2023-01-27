namespace HM.MiniGames.LinkGame
{
    public interface IGameSettings
    {
        int Width { get; }
        int Height { get; }
        int[] ContentIDs { get; }
    }
}
