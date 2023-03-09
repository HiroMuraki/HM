using HM.MiniGames.Common;
using HM.MiniGames.Extensions;
using HM.MiniGames.LinkGame.Enums;
using HM.MiniGames.LinkGame.Interfaces;
using System;
using System.Linq;

namespace HM.MiniGames.LinkGame
{
    public static class LinkGameHelper
    {
        public static Grid<TCell> Create<TCell>(
            IGameSettings settings,
            ICellGenerator<TCell> cellGenerator,
            IRandomGenerator randomGenerator)
            where TCell : ICell
        {
            int totalTokens = settings.Width * settings.Height;
            if (totalTokens % 2 != 0)
            {
                throw new ArgumentException($"Invalid layout size({settings.Width} x {settings.Height})");
            }

            var emptyMark = settings.ContentIDs.Min() - 1;
            var contentIDLayout = Grid<int>.Create(settings.Width, settings.Height);
            GridExtension.Fill(contentIDLayout, emptyMark);
            while (contentIDLayout.Any(c => c == emptyMark))
            {
                int contentID = settings.ContentIDs[randomGenerator.Range(0, settings.ContentIDs.Length)];
                var fixedCoords = contentIDLayout.FindCoordinates(c => c != emptyMark);
                GridExtension.RandomFill(contentIDLayout, contentID, 2, fixedCoords);
            }

            var result = Grid<TCell>.Create(settings.Width, settings.Height);
            foreach (var coord in result.GetCoordinates())
            {
                result[coord] = cellGenerator.CreateCell();
                result[coord].State = CellState.Idle;
                result[coord].Coordinate = coord;
                result[coord].ContentID = contentIDLayout[coord];
            }

            return result;
        }
        public static bool TryConnect<TCell>(Grid<TCell> gameBlocks, Coordinate start, Coordinate target, out Coordinate[] nodes)
            where TCell : ICell
        {
            var layoutMap = Grid<bool>.Create(gameBlocks.Width, gameBlocks.Height);
            foreach (var coord in gameBlocks.GetCoordinates())
            {
                layoutMap[coord] = gameBlocks[coord].State switch
                {
                    CellState.Idle => false,
                    CellState.None => true,
                    CellState.Matched => true,
                    _ => false
                };
            }
            return TryConnectCore(layoutMap, start, target, out nodes);
        }
        public static bool TryMatch<TCell>(Grid<TCell> gameBlocks, Coordinate start, Coordinate target)
            where TCell : ICell
        {
            return TryMatchCore(gameBlocks, start, target);
        }
        public static bool IsGameCompleted<TCell>(Grid<TCell> gameBlocks)
            where TCell : ICell
        {
            foreach (var token in gameBlocks)
            {
                if (token.State == CellState.Idle)
                {
                    return false;
                }
            }
            return true;
        }

        private static bool TryConnectCore(Grid<bool> accessableMap, Coordinate start, Coordinate target, out Coordinate[] nodes)
        {
            // 无转向连通检查
            if (TryZeroTurningLink(start, target, out nodes))
            {
                return true;
            }
            // 一次转向连通检查
            else if (TryOneTurningLink(start, target, out nodes))
            {
                return true;
            }
            // 二次转向连通检查
            else if (TryTwoTurningLink(start, target, out nodes))
            {
                return true;
            }
            else
            {
                nodes = Array.Empty<Coordinate>();
                return false;
            }

            // 测试纵向链接
            bool IsVLinked(Coordinate start, Coordinate target)
            {
                if (start.X != target.X)
                {
                    return false;
                }
                if (start == target)
                {
                    return true;
                }

                if (start.Y < target.Y)
                {
                    var testCoord = start.Up;
                    while (testCoord.Y < target.Y)
                    {
                        if (!accessableMap[testCoord])
                        {
                            return false;
                        }
                        testCoord = testCoord.Up;
                    }
                }
                else
                {
                    var testCoord = start.Down;
                    while (testCoord.Y > target.Y)
                    {
                        if (!accessableMap[testCoord])
                        {
                            return false;
                        }
                        testCoord = testCoord.Down;
                    }
                }
                return true;
            }
            // 测试横向链接
            bool IsHLinked(Coordinate start, Coordinate target)
            {
                if (start.Y != target.Y)
                {
                    return false;
                }
                if (start == target)
                {
                    return true;
                }

                if (start.X < target.X)
                {
                    var testCoord = start.Right;
                    while (testCoord.X < target.X)
                    {
                        if (!accessableMap[testCoord])
                        {
                            return false;
                        }
                        testCoord = testCoord.Right;
                    }
                }
                else
                {
                    var testCoord = start.Left;
                    while (testCoord.X > target.X)
                    {
                        if (!accessableMap[testCoord])
                        {
                            return false;
                        }
                        testCoord = testCoord.Left;
                    }
                }
                return true;
            }
            // 0转链接，只要纵向或者横向可以连接即可
            bool TryZeroTurningLink(Coordinate start, Coordinate target, out Coordinate[] nodes)
            {
                if (IsVLinked(start, target) || IsHLinked(start, target))
                {
                    nodes = new Coordinate[] { start, target };
                }
                else
                {
                    nodes = Array.Empty<Coordinate>();
                }

                return nodes.Length != 0;
            }
            // 1转链接，测试两点的两个纵横交点是否存在一个点可以联通
            bool TryOneTurningLink(Coordinate start, Coordinate target, out Coordinate[] nodes)
            {
                // 第一交点：横轴坐标为起点横轴坐标，纵轴坐标为目标点纵轴坐标
                var cross1 = new Coordinate(start.X, target.Y);
                // 第二交点：横轴坐标为目标点横轴坐标，纵轴坐标为起点点纵轴坐标
                var cross2 = new Coordinate(target.X, start.Y);
                // 测试第一交点连通性：检查起点与交点的纵向连通性+交点与目标点的横向连通性
                if (accessableMap[cross1] && IsVLinked(start, cross1) && IsHLinked(cross1, target))
                {
                    nodes = new Coordinate[] { start, cross1, target };
                }
                // 测试第二交点连通性：检查起点与交点的横向连通性+交点与目标点的纵向连通性
                else if (accessableMap[cross2] && IsHLinked(start, cross2) && IsVLinked(cross2, target))
                {
                    nodes = new Coordinate[] { start, cross2, target };
                }
                else
                {
                    nodes = Array.Empty<Coordinate>();
                }

                return nodes.Length != 0;
            }
            // 2转链接，遍历查找可以同时与start点和target点进行1转链接的点，若有多个点，则取总距离最短的点
            bool TryTwoTurningLink(Coordinate start, Coordinate target, out Coordinate[] nodes)
            {
                float minDist = -1;
                nodes = Array.Empty<Coordinate>();
                foreach (var coord in accessableMap.GetCoordinates())
                {
                    if (!accessableMap[coord])
                    {
                        continue;
                    }

                    // 如果解存在，首先验证当前坐标的距离合是否比当前最优解更优化
                    float dist = Coordinate.SqrDistance(start, coord) + Coordinate.SqrDistance(coord, target);
                    if (nodes.Length > 0 && dist > minDist)
                    {
                        continue;
                    }

                    if (TryOneTurningLink(start, coord, out var nodes1)
                        && TryOneTurningLink(coord, target, out var nodes2)
                        && TryZeroTurningLink(nodes1[1], nodes2[1], out _))
                    {
                        minDist = dist;
                        nodes = new Coordinate[] { start, nodes1[1], nodes2[1], target };
                    }
                }

                return nodes.Length != 0;
            }
        }
        private static bool TryMatchCore<TCell>(Grid<TCell> grid, Coordinate start, Coordinate target)
            where TCell : ICell
        {
            return grid[start].ContentID == grid[target].ContentID;
        }
    }
}