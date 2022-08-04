namespace HM.MiniGames.MinesweeperX
{
    public abstract class GameManagerBase
    {
        public virtual int Width => GameBlocks.Width;
        public virtual int Height => GameBlocks.Height;
        public virtual int MineCount { get; private set; }
        public virtual Grid<IGameBlock> GameBlocks
        {
            get => _gameBlocks;
            private set => _gameBlocks = value;
        }

        public virtual void CreateGame(IGameSetting setting)
        {
            GameBlocks = MinesweeperHelper.Create(setting);
            MinesweeperHelper.Shuffle(GameBlocks);
            MinesweeperHelper.UpdateMineCountInfo(GameBlocks);
            MineCount = setting.MineCount;
        }
        public virtual bool Open(Coordinate coordinate)
        {
            /* 仅当方块处于Open状态时才可打开，打开后游戏状态强制设置为GameStarted
             * 若打开的是空块区域（即周围雷块数量为0），则同时递归打开周围的方块
             * 若打开成功，则引发GameBlockActed事件并返回true，否则返回false */
            if (MinesweeperHelper.Open(GameBlocks, coordinate))
            {
                if (GameBlocks[coordinate].NearbyMineCount == 0)
                {
                    QuickOpen(coordinate);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public virtual bool QuickOpen(Coordinate coordinate)
        {
            var coords = MinesweeperHelper.GetQuickOpenCoordinates(GameBlocks, coordinate);
            foreach (var coord in coords)
            {
                GameBlocks[coord].BlockStatus = GameBlockStatus.Open;
            }
            return coords.Length > 0;
        }
        public virtual bool GuardOpen(Coordinate coordinate)
        {
            var guardedCoords = MinesweeperHelper.GetGuardOpenCoordinates(GameBlocks, coordinate);
            if (MinesweeperHelper.SetSafeCoordinates(GameBlocks, guardedCoords) >= 0)
            {
                MinesweeperHelper.UpdateMineCountInfo(GameBlocks);
                return Open(coordinate);
            }
            else
            {
                return false;
            }

        }
        public virtual bool Flag(Coordinate coordinate)
        {
            return MinesweeperHelper.Flag(GameBlocks, coordinate);
        }
        public virtual bool Unflag(Coordinate coordinate)
        {
            return MinesweeperHelper.Unflag(GameBlocks, coordinate);
        }
        public virtual bool IsGameCompleted()
        {
            return MinesweeperHelper.CheckIfGameCompleted(GameBlocks);
        }

        public GameManagerBase(IRandomGenerator randomGenerator, IGameBlockGenerator blockGenerator)
        {
            MinesweeperHelper = new MinesweeperHelper(randomGenerator, blockGenerator);
        }

        protected MinesweeperHelper MinesweeperHelper;
        protected Grid<IGameBlock> _gameBlocks = Grid<IGameBlock>.Empty;
    }
}
