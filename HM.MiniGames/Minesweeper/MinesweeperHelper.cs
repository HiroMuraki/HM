using System;
using System.Linq;
using System.Collections.Generic;

namespace HM.MiniGames.Minesweeper
{
    public sealed class MinesweeperHelper
    {
        /// <summary>
        /// 八个方向的偏移值
        /// </summary>
        public static readonly Coordinate[] NearbyDelta = new Coordinate[8]
        {
        #pragma warning disable format
            (-1,-1), (0,-1), (1,-1),
            (-1, 0),         (1, 0),
            (-1, 1), (0, 1), (1, 1),
        #pragma warning restore format
        };

        /// <summary>
        /// 随机数发生器
        /// </summary>
        public IRandomGenerator RandomGenerator { get; }
        /// <summary>
        /// 游戏方块生成器
        /// </summary>
        public IGameBlockGenerator GameBlockGenerator { get; }

        /// <summary>
        /// 创建初始化的游戏布局
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="gameBlockGenerator"></param>
        /// <returns>创建结果</returns>
        public Grid<IGameBlock> Create(IGameSetting setting)
        {
            var gameBlocks = Grid<IGameBlock>.Create(setting.Width, setting.Height);
            int totalMineCount = setting.MineCount;
            int currentMineCount = 0;
            for (int y = 0; y < gameBlocks.Height; y++)
            {
                for (int x = 0; x < gameBlocks.Width; x++)
                {
                    var block = GameBlockGenerator.GetGameBlock();
                    block.State = GameBlockState.Closed;
                    if (currentMineCount < totalMineCount)
                    {
                        block.Type = GameBlockType.Mine;
                        currentMineCount++;
                    }
                    else
                    {
                        block.Type = GameBlockType.Blank;
                    }
                    gameBlocks[x, y] = block;
                }
            }
            return gameBlocks;
        }
        /// <summary>
        /// 打乱Blank/Mine块的布局
        /// </summary>
        /// <param name="gameBlocks"></param>
        public void Shuffle(Grid<IGameBlock> gameBlocks)
        {
            int total = gameBlocks.Width * gameBlocks.Height;
            for (int y = 0; y < gameBlocks.Height; y++)
            {
                for (int x = 0; x < gameBlocks.Width; x++)
                {
                    int next = RandomGenerator.Range(y * gameBlocks.Width + x, total);
                    int nextX = next % gameBlocks.Width;
                    int nextY = next / gameBlocks.Width;
                    SwapBlockType(gameBlocks[x, y], gameBlocks[nextX, nextY]);
                }
            }
        }
        /// <summary>
        /// 设置安全区域，将安全区域的Mine块用其他区域的Blank块替换
        /// </summary>
        /// <param name="gameBlocks"></param>
        /// <param name="coordinates"></param>
        /// <returns>受影响的方块数，若为-1，则表示没有足够的Blank块用于替换Mine块</returns>
        public int SetSafeCoordinates(Grid<IGameBlock> gameBlocks, IEnumerable<Coordinate> coordinates)
        {
            /* 获取所有目标坐标中的Mine块，获取Mine块组
             * 获取所有除coordinates外的Blank + Closed的方块，获取Blank块组
             * 若Mine块组大小小于Blank组，则将每个Mine块与随机一个Blank块交换 */
            var coords = coordinates.ToArray();
            var mineCoords = coords.Where(c => gameBlocks[c].Type == GameBlockType.Mine).ToArray();
            if (!mineCoords.Any())
            {
                return 0;
            }

            var blankCoords = gameBlocks
                .GetCoordinates()
                .Where(c => gameBlocks[c].Type == GameBlockType.Blank && gameBlocks[c].State == GameBlockState.Closed)
                .Except(coords)
                .ToList();

            if (blankCoords.Count > mineCoords.Length)
            {
                for (int i = 0; i < mineCoords.Length; i++)
                {
                    var rndBlankCoord = blankCoords[RandomGenerator.Range(0, blankCoords.Count)];
                    blankCoords.Remove(rndBlankCoord);
                    SwapBlockType(gameBlocks[mineCoords[i]], gameBlocks[rndBlankCoord]);
                }
                return mineCoords.Length;
            }
            else
            {
                return -1;
            }

        }
        /// <summary>
        /// 获取指定坐标附近的有效坐标
        /// </summary>
        /// <param name="gameBlocks"></param>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public Coordinate[] GetNearybyCoordinates(Grid<IGameBlock> gameBlocks, Coordinate coordinate)
        {
            return NearbyDelta
                .Select(c => coordinate + c)
                .Where(gameBlocks.IsValidCoordinate)
                .ToArray();
        }
        /// <summary>
        /// 更新布局中各个GameBlock的MineCout信息
        /// </summary>
        /// <param name="gameBlocks"></param>
        public void UpdateMineCountInfo(Grid<IGameBlock> gameBlocks)
        {
            foreach (var coord in gameBlocks.GetCoordinates())
            {
                gameBlocks[coord].NearbyMineCount =
                    GetNearybyCoordinates(gameBlocks, coord)
                    .Where(c => gameBlocks[c].Type == GameBlockType.Mine)
                    .Count();
            }
        }
        /// <summary>
        /// 检查游戏是否完成（即所有的Blank块都处于Open状态）
        /// </summary>
        /// <param name="gameBlocks"></param>
        /// <returns></returns>
        public bool CheckIfGameCompleted(Grid<IGameBlock> gameBlocks)
        {
            return !gameBlocks.Any(b => b.Type == GameBlockType.Blank
                                     && b.State != GameBlockState.Open);
        }
        /// <summary>
        /// 打开坐标指定方块
        /// </summary>
        /// <param name="gameBlocks"></param>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public bool Open(Grid<IGameBlock> gameBlocks, Coordinate coordinate)
        {
            if (!gameBlocks.IsValidCoordinate(coordinate)) return false;

            if (gameBlocks[coordinate].State == GameBlockState.Closed)
            {
                gameBlocks[coordinate].State = GameBlockState.Open;
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 获取快开下将会被打开的方块的坐标
        /// </summary>
        /// <param name="gameBlocks"></param>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public Coordinate[] GetQuickOpenCoordinates(Grid<IGameBlock> gameBlocks, Coordinate coordinate)
        {
            bool[,] openMap = new bool[gameBlocks.Width, gameBlocks.Height];
            return GetQuickOpenCoordinatesCore(gameBlocks, coordinate, openMap);
        }
        /// <summary>
        /// 获取保护打开模式下会受到影响的方块的坐标（自身坐标+周围八个坐标，即保证其必定是空区）
        /// </summary>
        /// <param name="gameBlocks"></param>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public Coordinate[] GetGuardOpenCoordinates(Grid<IGameBlock> gameBlocks, Coordinate coordinate)
        {
            var guaredCoords =
                GetNearybyCoordinates(gameBlocks, coordinate)
                .Append(coordinate)
                .Where(gameBlocks.IsValidCoordinate)
                .ToArray();

            return guaredCoords;
        }
        /// <summary>
        /// 为指定坐标的方块标旗
        /// </summary>
        /// <param name="gameBlocks"></param>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public bool Flag(Grid<IGameBlock> gameBlocks, Coordinate coordinate)
        {
            if (!gameBlocks.IsValidCoordinate(coordinate)) return false;

            if (gameBlocks[coordinate].State == GameBlockState.Closed)
            {
                gameBlocks[coordinate].State = GameBlockState.Flagged;
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 取消对指定坐标方块的标旗
        /// </summary>
        /// <param name="gameBlocks"></param>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public bool Unflag(Grid<IGameBlock> gameBlocks, Coordinate coordinate)
        {
            if (!gameBlocks.IsValidCoordinate(coordinate)) return false;

            if (gameBlocks[coordinate].State == GameBlockState.Flagged)
            {
                gameBlocks[coordinate].State = GameBlockState.Closed;
                return true;
            }
            else
            {
                return false;
            }
        }

        public MinesweeperHelper(IRandomGenerator random, IGameBlockGenerator gameBlockGenerator)
        {
            RandomGenerator = random;
            GameBlockGenerator = gameBlockGenerator;
        }

        private Coordinate[] GetQuickOpenCoordinatesCore(Grid<IGameBlock> gameBlocks, Coordinate coordinate, bool[,] openMap)
        {
            if (!gameBlocks.IsValidCoordinate(coordinate)) return Array.Empty<Coordinate>();
            if (openMap[coordinate.X, coordinate.Y]) return Array.Empty<Coordinate>();

            /*  获取目标坐标周围至多八个坐标，统计这些坐标中标记了Flag的方块数，并记录处于Closed状态的方块
             *  如果标记为了Flag的方块数量大于目标坐标的MineCount指示，则打开本方块的同时将记录的Closed状态的方块递归打开
             *  （这里使用一个二维bool数组openMap来记录缓存Open状态）*/
            var flaggedCount = 0;
            var nearbyCoords = GetNearybyCoordinates(gameBlocks, coordinate).ToArray();
            var openableCoords = new List<Coordinate>(NearbyDelta.Length);
            foreach (var coord in nearbyCoords)
            {
                switch (gameBlocks[coord].State)
                {
                    case GameBlockState.Closed:
                        if (!openMap[coord.X, coord.Y])
                        {
                            openableCoords.Add(coord);
                        }
                        break;
                    case GameBlockState.Flagged:
                        flaggedCount++;
                        break;
                    case GameBlockState.Open:
                    case GameBlockState.Undefined:
                    default:
                        break;
                }
            }

            var result = new LinkedList<Coordinate>();
            openMap[coordinate.X, coordinate.Y] = true;
            result.AddLast(coordinate);

            if (flaggedCount >= gameBlocks[coordinate].NearbyMineCount)
            {
                foreach (var coord in openableCoords)
                {
                    foreach (var sCoord in GetQuickOpenCoordinatesCore(gameBlocks, coord, openMap))
                    {
                        result.AddLast(sCoord);
                    }
                }
            }

            return result.ToArray();
        }
        private static void SwapBlockType(IGameBlock a, IGameBlock b)
        {
            (a.Type, b.Type) = (b.Type, a.Type);
        }
    }
}
