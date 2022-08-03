namespace HM.MiniGames.MinesweeperX
{
    public abstract class GameManagerBase
    {
        public event EventHandler<GameBlockActedEventArgs>? GameBlockActed;

        public virtual IGameBlockGenerator GameBlockGenerator => _gameBlockGenerator;
        public virtual int Width { get; private set; }
        public virtual int Height { get; private set; }
        public virtual int MineCount { get; private set; }
        public virtual GameStatus GameStatus
        {
            get => _gameStatus;
            private set => _gameStatus = value;
        }
        public virtual Grid<IGameBlock> GameBlocks
        {
            get => _gameBlocks;
            private set => _gameBlocks = value;
        }

        public virtual void CreateGame(IGameSetting setting)
        {
            GameStatus = GameStatus.Preparing;
            Width = setting.Width;
            Height = setting.Height;
            MineCount = setting.MineCount;
            GameBlocks = Grid<IGameBlock>.Create(Width, Height);
            int mineCount = 0;
            foreach (var coord in GameBlocks.GetCoordinates())
            {
                var gameBlock = GameBlockGenerator.GetGameBlock();
                gameBlock.BlockStatus = GameBlockStatus.Closed;
                if (mineCount < MineCount)
                {
                    gameBlock.BlockType = GameBlockType.Mine;
                    mineCount++;
                }
                else
                {
                    gameBlock.BlockType = GameBlockType.Blank;
                }
                GameBlocks[coord] = gameBlock;
            }
            ShuffleLayout();
            UpdateMineCountInfo();
            GameStatus = GameStatus.Prepared;
        }
        public virtual bool Open(Coordinate coordinate)
        {
            if (!IsValidCoordinate(coordinate)) return false;

            /* 仅当方块处于Open状态时才可打开，打开后游戏状态强制设置为GameStarted
             * 若打开的是空块区域（即周围雷块数量为0），则同时递归打开周围的方块
             * 若打开成功，则引发GameBlockActed事件并返回true，否则返回false */
            if (GameBlocks[coordinate].BlockStatus == GameBlockStatus.Closed)
            {
                GameBlocks[coordinate].BlockStatus = GameBlockStatus.Open;
                if (GameStatus != GameStatus.Started)
                {
                    GameStatus = GameStatus.Started;
                }
                if (GameBlocks[coordinate].NearbyMineCount == 0)
                {
                    QuickOpen(coordinate);
                }
                OnGameBlockActed(GameBlockAction.Open, coordinate);
                return true;
            }
            else
            {
                return false;
            }
        }
        public virtual bool QuickOpen(Coordinate coordinate)
        {
            if (!IsValidCoordinate(coordinate)) return false;

            /* 获取周围八个方向处于Closed状态的方块坐标组以及处于Flagged状态的方块坐标组
             * 若Flagged的数量小于该方块周围的雷数指示，则直接返回false，
             * 否则递归打开周围处于Closed状态的方块 */

            int count = 0;
            var openableCoords = new List<Coordinate>(_nearbyDelta.Length);
            var coords = GetNearybyCoordinates(coordinate);
            foreach (var dCoord in coords)
            {
                switch (GameBlocks[dCoord].BlockStatus)
                {
                    case GameBlockStatus.Flagged:
                        count++;
                        break;
                    case GameBlockStatus.Closed:
                        openableCoords.Add(dCoord);
                        break;
                    default:
                    case GameBlockStatus.Open:
                        break;
                }
            }

            if (GameBlocks[coordinate].NearbyMineCount > count)
            {
                return false;
            }

            foreach (var coord in openableCoords)
            {
                Open(coord);
                if (GameBlocks[coord].NearbyMineCount == 0)
                {
                    QuickOpen(coord);
                }
            }

            return true;
        }
        public virtual bool GuardOpen(Coordinate coordinate)
        {
            if (!IsValidCoordinate(coordinate)) return false;

            /* 获取目标坐标及其四周八个方向的坐标的位置组，
             * 将这些坐标设置为空块后再打开 */
            var guaredCoords =
                GetNearybyCoordinates(coordinate)
                .Append(coordinate)
                .Where(IsValidCoordinate);

            SetSafeCoordinates(guaredCoords);
            UpdateMineCountInfo();

            return Open(coordinate);
        }
        public virtual bool Flag(Coordinate coordinate)
        {
            if (!IsValidCoordinate(coordinate)) return false;

            if (GameBlocks[coordinate].BlockStatus == GameBlockStatus.Closed)
            {
                GameBlocks[coordinate].BlockStatus = GameBlockStatus.Flagged;
                OnGameBlockActed(GameBlockAction.Flag, coordinate);
                return true;
            }
            else
            {
                return false;
            }
        }
        public virtual bool Unflag(Coordinate coordinate)
        {
            if (!IsValidCoordinate(coordinate)) return false;

            if (GameBlocks[coordinate].BlockStatus == GameBlockStatus.Flagged)
            {
                GameBlocks[coordinate].BlockStatus = GameBlockStatus.Closed;
                OnGameBlockActed(GameBlockAction.Unflag, coordinate);
                return true;
            }
            else
            {
                return false;
            }
        }
        public virtual bool IsGameCompleted()
        {
            foreach (var item in GameBlocks)
            {
                if (item.BlockStatus != GameBlockStatus.Open)
                {
                    return false;
                }
            }
            return true;
        }

        public GameManagerBase(IGameBlockGenerator gameBlockGenerator)
        {
            _gameBlockGenerator = gameBlockGenerator;
        }

        protected static readonly Coordinate[] _nearbyDelta = new Coordinate[8]
        {
            #pragma warning disable format
            (-1,-1), (0,-1), (1,-1),
            (-1, 0),         (1, 0),
            (-1, 1), (0, 1), (1, 1),
            #pragma warning restore format
        };
        protected readonly IGameBlockGenerator _gameBlockGenerator;
        protected Random _random = new();
        protected Grid<IGameBlock> _gameBlocks = Grid<IGameBlock>.Empty;
        protected GameStatus _gameStatus = GameStatus.NotStarted;
        protected Coordinate[] GetNearybyCoordinates(Coordinate coordinate)
        {
            if (!IsValidCoordinate(coordinate)) return Array.Empty<Coordinate>();

            return _nearbyDelta.Select(c => coordinate + c).Where(IsValidCoordinate).ToArray();
        }
        protected bool SetSafeCoordinate(Coordinate coordinate)
        {
            // 如果已经是安全块则跳过操作
            if (GameBlocks[coordinate].BlockType == GameBlockType.Blank)
            {
                return false;
            }

            /* 选取剩下的处于Closed状态的Blank块
             * 随机挑选一个与目标块进行交换*/
            var blankClosedBlockCoords =
                GameBlocks
                .GetCoordinates()
                .Where(c => GameBlocks[c].BlockType == GameBlockType.Blank && GameBlocks[c].BlockStatus == GameBlockStatus.Closed)
                .ToArray();

            var nextCoord = blankClosedBlockCoords[_random.Next(0, blankClosedBlockCoords.Length)];
            SwapGameBlockProperties(GameBlocks[coordinate], GameBlocks[nextCoord]);
            return true;
        }
        protected int SetSafeCoordinates(IEnumerable<Coordinate> coordinates)
        {
            /* 获取所有目标坐标中的Mine块，获取Mine块组
             * 获取所有除coordinates外的Blank + Closed的方块，获取Blank块组
             * 若Mine块组大小小于Blank组，则将每个Mine块与随机一个Blank块交换 */
            var coords = coordinates.ToArray();
            var mineCoords = coords.Where(c => GameBlocks[c].BlockType == GameBlockType.Mine).ToArray();
            if (!mineCoords.Any())
            {
                return 0;
            }

            var blankCoords = GameBlocks
                .GetCoordinates()
                .Where(c => GameBlocks[c].BlockType == GameBlockType.Blank && GameBlocks[c].BlockStatus == GameBlockStatus.Closed)
                .Except(coords)
                .ToList();

            if (blankCoords.Count > mineCoords.Length)
            {
                for (int i = 0; i < mineCoords.Length; i++)
                {
                    var rndBlankCoord = blankCoords[_random.Next(0, blankCoords.Count)];
                    blankCoords.Remove(rndBlankCoord);
                    SwapGameBlockProperties(GameBlocks[mineCoords[i]], GameBlocks[rndBlankCoord]);
                }
                return mineCoords.Length;
            }
            else
            {
                return -1;
            }

        }
        protected bool IsValidCoordinate(Coordinate coordinate)
        {
            return coordinate.X >= 0
                && coordinate.X < GameBlocks.Width
                && coordinate.Y >= 0
                && coordinate.Y < GameBlocks.Height;
        }
        protected void ShuffleLayout()
        {
            var items = GameBlocks.ToArray();
            for (int i = 0; i < items.Length; i++)
            {
                int next = _random.Next(i, items.Length);
                (items[i], items[next]) = (items[next], items[i]);
            }

            var coords = GameBlocks.GetCoordinates().ToArray();
            for (int i = 0; i < coords.Length; i++)
            {
                GameBlocks[coords[i]] = items[i];
            }
        }
        protected void UpdateMineCountInfo()
        {
            foreach (var coord in GameBlocks.GetCoordinates())
            {
                GameBlocks[coord].NearbyMineCount =
                    GetNearybyCoordinates(coord)
                    .Where(c => GameBlocks[c].BlockType == GameBlockType.Mine)
                    .Count();
            }
        }
        protected void OnGameBlockActed(GameBlockAction action, Coordinate coordinate)
        {
            GameBlockActed?.Invoke(this, new GameBlockActedEventArgs()
            {
                Action = action,
                GameBlock = GameBlocks[coordinate]
            });
        }
        protected virtual void SwapGameBlockProperties(IGameBlock left, IGameBlock right)
        {
            (left.NearbyMineCount, right.NearbyMineCount) = (right.NearbyMineCount, left.NearbyMineCount);
            (left.BlockStatus, right.BlockStatus) = (right.BlockStatus, left.BlockStatus);
            (left.BlockType, right.BlockType) = (right.BlockType, left.BlockType);
        }
    }
}
