using HM.MiniGames;

namespace HM.MiniGames.LinkGame
{
    public class GameManagerBase
    {
        public void SelectToken(Grid<ICell> gameTokens, Coordinate coord)
        {
            return;
            //if (Grid[coord].Status == TokenStatus.Idle)
            //{
            //    if (ReferenceEquals(_heldToken, _noToken))
            //    {
            //        _heldToken = Grid[coord];
            //        _heldToken.Status = TokenStatus.Selected;
            //    }
            //    else
            //    {
            //        if (_gameHelper.TryConnect(_heldToken.Coordinate, coord, out var nodes))
            //        {
            //            if (_gameHelper.TryMatch(_heldToken.Coordinate, coord))
            //            {
            //                _heldToken.Status = TokenStatus.Matched;
            //                Grid[coord].Status = TokenStatus.Matched;
            //                OnTokenMatched(_heldToken, Grid[coord], nodes);
            //            }
            //            else
            //            {
            //                _heldToken.Status = TokenStatus.Idle;
            //                Grid[coord].Status = TokenStatus.Idle;
            //            }
            //        }
            //        _heldToken = _noToken;
            //    }
            //}
            //else if (_heldToken.Coordinate == coord)
            //{
            //    _heldToken = _noToken;
            //    _heldToken.Status = TokenStatus.Idle;
            //}
        }
    }
}
