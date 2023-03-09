namespace HM.MiniGames.LinkGame.Interfaces
{
    public interface IGameSettings
    {
        int Width { get; }
        int Height { get; }
        int[] ContentIDs { get; }
    }
}
