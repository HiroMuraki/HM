#nullable enable
using System;
using System.Linq;
using System.Collections.Generic;

namespace HM.MiniGames.Minesweeper
{
    public static class MinesweeperHelper
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
        /// 创建初始化的游戏布局
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="gameBlockGenerator"></param>
        /// <returns>创建结果</returns>
        public static Grid<TOutCell> Create<TOutCell>(IGameSetting setting, IGameCellGenerator<TOutCell> cellGenerator) where TOutCell : IGameCell
        {
            var gameCells = Grid<TOutCell>.Create(setting.Width, setting.Height);
            int totalMineCount = setting.MineCount;
            int currentMineCount = 0;
            for (int y = 0; y < gameCells.Height; y++)
            {
                for (int x = 0; x < gameCells.Width; x++)
                {
                    var block = cellGenerator.GetGameCell();
                    block.State = GameCellState.Closed;
                    if (currentMineCount < totalMineCount)
                    {
                        block.Type = GameCellType.Mine;
                        currentMineCount++;
                    }
                    else
                    {
                        block.Type = GameCellType.Blank;
                    }
                    gameCells[x, y] = block;
                }
            }
            return gameCells;
        }
        /// <summary>
        /// 打乱Blank/Mine块的布局
        /// </summary>
        /// <param name="gameCells"></param>
        public static void Shuffle(Grid<IGameCell> gameCells, IRandomGenerator randomGenerator)
        {
            int total = gameCells.Width * gameCells.Height;
            for (int y = 0; y < gameCells.Height; y++)
            {
                for (int x = 0; x < gameCells.Width; x++)
                {
                    int next = randomGenerator.Range(y * gameCells.Width + x, total);
                    int nextX = next % gameCells.Width;
                    int nextY = next / gameCells.Width;
                    SwapBlockType(gameCells[x, y], gameCells[nextX, nextY]);
                }
            }
        }
        /// <summary>
        /// 设置安全区域，将安全区域的Mine块用其他区域的Blank块替换
        /// </summary>
        /// <param name="gameCells"></param>
        /// <param name="coordinates"></param>
        /// <returns>受影响的方块数，若为-1，则表示没有足够的Blank块用于替换Mine块</returns>
        public static int SetSafeCoordinates(Grid<IGameCell> gameCells, IEnumerable<Coordinate> coordinates, IRandomGenerator randomGenerator)
        {
            /* 获取所有目标坐标中的Mine块，获取Mine块组
             * 获取所有除coordinates外的Blank + Closed的方块，获取Blank块组
             * 若Mine块组大小小于Blank组，则将每个Mine块与随机一个Blank块交换 */
            var coords = coordinates.ToArray();
            var mineCoords = coords.Where(c => gameCells[c].Type == GameCellType.Mine).ToArray();
            if (!mineCoords.Any())
            {
                return 0;
            }

            var blankCoords = gameCells
                .GetCoordinates()
                .Where(c => gameCells[c].Type == GameCellType.Blank && gameCells[c].State == GameCellState.Closed)
                .Except(coords)
                .ToList();

            if (blankCoords.Count > mineCoords.Length)
            {
                for (int i = 0; i < mineCoords.Length; i++)
                {
                    var rndBlankCoord = blankCoords[randomGenerator.Range(0, blankCoords.Count)];
                    blankCoords.Remove(rndBlankCoord);
                    SwapBlockType(gameCells[mineCoords[i]], gameCells[rndBlankCoord]);
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
        /// <param name="gameCells"></param>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public static Coordinate[] GetNearybyCoordinates(Grid<IGameCell> gameCells, Coordinate coordinate)
        {
            return NearbyDelta
                .Select(c => coordinate + c)
                .Where(gameCells.IsValidCoordinate)
                .ToArray();
        }
        /// <summary>
        /// 更新布局中各个GameBlock的MineCout信息
        /// </summary>
        /// <param name="gameCells"></param>
        public static void UpdateMineCountInfo(Grid<IGameCell> gameCells)
        {
            foreach (var coord in gameCells.GetCoordinates())
            {
                gameCells[coord].NearbyMineCount =
                    GetNearybyCoordinates(gameCells, coord)
                    .Where(c => gameCells[c].Type == GameCellType.Mine)
                    .Count();
            }
        }
        /// <summary>
        /// 检查游戏状态
        /// </summary>
        /// <param name="gameCells"></param>
        /// <returns></returns>
        public static GameResult CheckIfGameCompleted(Grid<IGameCell> gameCells)
        {
            /* 若存在任何一个处于Open状态的Mine块，则返回Fail，
             * 若存在任何一个处于Closed状态的Blank块，则返回Unknow，
             * 否则即所有的Mine块都处于Closed状态，Blank块都处于Open状态，视为Success*/
            foreach (var cell in gameCells)
            {
                if (cell.State == GameCellState.Open && cell.Type == GameCellType.Mine)
                {
                    return GameResult.Fail;
                }
                else if (cell.State == GameCellState.Closed && cell.Type == GameCellType.Blank)
                {
                    return GameResult.Unknow;
                }
            }
            return GameResult.Success;
        }
        /// <summary>
        /// 打开坐标指定方块，若打开成功返回true，否则若坐标无效或Cell的状态不为Closed则返回false
        /// </summary>
        /// <param name="gameCells"></param>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public static GameCellOpenResult Open(Grid<IGameCell> gameCells, Coordinate coordinate)
        {
            if (!gameCells.IsValidCoordinate(coordinate))
            {
                return GameCellOpenResult.None;
            }
            if (gameCells[coordinate].State == GameCellState.Open)
            {
                return GameCellOpenResult.None;
            }

            if (gameCells[coordinate].State is GameCellState.Closed or GameCellState.Held)
            {
                gameCells[coordinate].State = GameCellState.Open;
                if (gameCells[coordinate].Type == GameCellType.Blank)
                {
                    return GameCellOpenResult.Open;
                }
                else if (gameCells[coordinate].Type == GameCellType.Mine)
                {
                    return GameCellOpenResult.HitMine;
                }

                return GameCellOpenResult.Open;
            }

            return GameCellOpenResult.None;
        }
        /// <summary>
        /// 获取快开下将会被打开的方块的坐标
        /// </summary>
        /// <param name="gameCells"></param>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public static Coordinate[] GetQuickOpenCoordinates(Grid<IGameCell> gameCells, Coordinate coordinate)
        {
            bool[,] openMap = new bool[gameCells.Width, gameCells.Height];
            return GetQuickOpenCoordinatesCore(gameCells, coordinate, openMap);
        }
        /// <summary>
        /// 获取保护打开模式下会受到影响的方块的坐标（即自身坐标+周围八个坐标）
        /// </summary>
        /// <param name="gameCells"></param>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public static Coordinate[] GetGuardOpenCoordinates(Grid<IGameCell> gameCells, Coordinate coordinate)
        {
            var guaredCoords =
                GetNearybyCoordinates(gameCells, coordinate)
                .Append(coordinate)
                .Where(gameCells.IsValidCoordinate)
                .ToArray();

            return guaredCoords;
        }
        /// <summary>
        /// 为指定坐标的方块标旗
        /// </summary>
        /// <param name="gameCells"></param>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public static bool Flag(Grid<IGameCell> gameCells, Coordinate coordinate)
        {
            if (!gameCells.IsValidCoordinate(coordinate)) return false;

            if (gameCells[coordinate].State == GameCellState.Closed)
            {
                gameCells[coordinate].State = GameCellState.Flagged;
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
        /// <param name="gameCells"></param>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public static bool Unflag(Grid<IGameCell> gameCells, Coordinate coordinate)
        {
            if (!gameCells.IsValidCoordinate(coordinate)) return false;

            if (gameCells[coordinate].State == GameCellState.Flagged)
            {
                gameCells[coordinate].State = GameCellState.Closed;
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 揭示方块类型
        /// </summary>
        /// <param name="gameCells"></param>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public static bool Proclaim(Grid<IGameCell> gameCells, Coordinate coordinate)
        {
            if (!gameCells.IsValidCoordinate(coordinate)) return false;

            gameCells[coordinate].State = GameCellState.Proclaimed;
            return true;
        }

        private static Coordinate[] GetQuickOpenCoordinatesCore(Grid<IGameCell> gameCells, Coordinate coordinate, bool[,] openMap)
        {
            if (!gameCells.IsValidCoordinate(coordinate)) return Array.Empty<Coordinate>();
            if (openMap[coordinate.X, coordinate.Y]) return Array.Empty<Coordinate>();

            /*  获取目标坐标周围至多八个坐标，统计这些坐标中标记了Flag的方块数，并记录处于Closed状态的方块
             *  如果标记为了Flag的方块数量大于目标坐标的MineCount指示，则打开本方块的同时将记录的Closed状态的方块递归打开
             *  （这里使用一个二维bool数组openMap来记录缓存Open状态）*/
            var flaggedCount = 0;
            var nearbyCoords = GetNearybyCoordinates(gameCells, coordinate).ToArray();
            var openableCoords = new List<Coordinate>(NearbyDelta.Length);
            foreach (var coord in nearbyCoords)
            {
                switch (gameCells[coord].State)
                {
                    case GameCellState.Closed:
                        if (!openMap[coord.X, coord.Y])
                        {
                            openableCoords.Add(coord);
                        }
                        break;
                    case GameCellState.Flagged:
                        flaggedCount++;
                        break;
                    case GameCellState.Open:
                    case GameCellState.Undefined:
                    default:
                        break;
                }
            }

            var result = new LinkedList<Coordinate>();
            openMap[coordinate.X, coordinate.Y] = true;
            result.AddLast(coordinate);

            if (flaggedCount == gameCells[coordinate].NearbyMineCount)
            {
                foreach (var coord in openableCoords)
                {
                    foreach (var sCoord in GetQuickOpenCoordinatesCore(gameCells, coord, openMap))
                    {
                        result.AddLast(sCoord);
                    }
                }
            }

            return result.ToArray();
        }
        private static void SwapBlockType(IGameCell a, IGameCell b)
        {
            (a.Type, b.Type) = (b.Type, a.Type);
        }
    }
}
