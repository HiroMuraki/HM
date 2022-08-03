using HM.MiniGames.LinkGame;

namespace HM.MiniGames.LinkGame
{
    public class GameStatusChangedEventArgs : EventArgs
    {
        public GameStatus Status { get; }

        public GameStatusChangedEventArgs(GameStatus status)
        {
            Status = status;
        }
    }
}
