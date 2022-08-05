using System.Text;
using System;
using System.Linq;

namespace HM.MiniGames
{
    internal static class GridExtension
    {
        internal static T[] ToArray<T>(this Grid<T> grid)
        {
            var result = new T[grid.Width * grid.Height];
            for (int y = 0; y < grid.Height; y++)
            {
                int offsetY = y * grid.Height;
                for (int x = 0; x < grid.Width; x++)
                {
                    result[offsetY + x] = grid[x, y];
                }
            }
            return result;
        }
        internal static Grid<T> Shrink<T>(this Grid<T> grid, Directions directions, int shrinkSize)
        {
            if (shrinkSize < 0)
            {
                throw new ArgumentException("shrink size should be larger than zero");
            }

            int newWidth = grid.Width;
            int newHeight = grid.Height;
            int offsetX = 0;
            int offsetY = 0;

            if ((directions & Directions.Up) == Directions.Up)
            {
                offsetY += shrinkSize;
                newHeight -= shrinkSize;
            }
            if ((directions & Directions.Down) == Directions.Down)
            {
                newHeight -= shrinkSize;
            }
            if ((directions & Directions.Left) == Directions.Left)
            {
                offsetX += shrinkSize;
                newWidth -= shrinkSize;
            }
            if ((directions & Directions.Right) == Directions.Right)
            {
                newWidth -= shrinkSize;
            }

            if (newWidth <= 0 || newHeight <= 0)
            {
                return Grid<T>.Empty;
            }

            var result = Grid<T>.Create(newWidth, newHeight);
            for (int y = 0; y < newHeight; y++)
            {
                for (int x = 0; x < newHeight; x++)
                {
                    result[x, y] = grid[y + offsetY, x + offsetX];
                }
            }

            return result;
        }
        internal static Grid<T> Expand<T>(this Grid<T> grid, Directions directions, int expandSize)
        {
            if (expandSize < 0)
            {
                throw new ArgumentException("expand size should be larger than zero");
            }

            int newWidth = grid.Width;
            int newHeight = grid.Height;
            int offsetX = 0;
            int offsetY = 0;

            if ((directions & Directions.Up) == Directions.Up)
            {
                offsetY += expandSize;
                newHeight += expandSize;
            }
            if ((directions & Directions.Down) == Directions.Down)
            {
                newHeight += expandSize;
            }
            if ((directions & Directions.Left) == Directions.Left)
            {
                offsetX += expandSize;
                newWidth += expandSize;
            }
            if ((directions & Directions.Right) == Directions.Right)
            {
                newWidth += expandSize;
            }

            var result = Grid<T>.Create(newWidth, newHeight);
            if ((directions & Directions.Up) == Directions.Up)
            {
                for (int y = 0; y < expandSize; y++)
                {
                    for (int x = 0; x < newWidth; x++)
                    {
                        result[x, y] = default!;
                    }
                }
            }
            if ((directions & Directions.Down) == Directions.Down)
            {
                for (int y = 0; y < expandSize; y++)
                {
                    for (int x = 0; x < newWidth; x++)
                    {
                        result[x, newHeight - 1 - y] = default!;
                    }
                }
            }
            if ((directions & Directions.Left) == Directions.Left)
            {
                for (int x = 0; x < expandSize; x++)
                {
                    for (int y = 0; y < newHeight; y++)
                    {
                        result[x, y] = default!;
                    }
                }
            }
            if ((directions & Directions.Right) == Directions.Right)
            {
                for (int x = 0; x < expandSize; x++)
                {
                    for (int y = 0; y < newHeight; y++)
                    {
                        result[newWidth - 1 - x, y] = default!;
                    }
                }
            }

            for (int y = 0; y < grid.Width; y++)
            {
                for (int x = 0; x < grid.Height; x++)
                {
                    result[x + offsetX, y + offsetY] = grid[x, y];
                }
            }

            return result;
        }
        internal static bool TryFindCoordinates<T>(this Grid<T> grid, Predicate<T> predicate, out Coordinate[] result)
        {
            result = (from coord in grid.GetCoordinates()
                      where predicate(grid[coord])
                      select coord).ToArray();

            return result.Length != 0;
        }
        internal static Coordinate[] FindCoordinates<T>(this Grid<T> grid, Predicate<T> predicate)
        {
            TryFindCoordinates(grid, predicate, out var result);
            return result;
        }
        internal static void Fill<T>(this Grid<T> grid, T[] values)
        {
            Fill(grid, values, Array.Empty<Coordinate>());
        }
        internal static void Fill<T>(this Grid<T> grid, T[] values, Coordinate[] ignoredCoords)
        {
            if (grid.Width == 0 || grid.Height == 0 || values.Length == 0)
            {
                return;
            }

            var coords = grid.GetCoordinates().Except(ignoredCoords).ToArray();

            int maxCycle = coords.Length < values.Length ? coords.Length : values.Length;
            for (int i = 0; i < maxCycle; i++)
            {
                grid[coords[i]] = values[i];
            }
        }
        internal static void Fill<T>(this Grid<T> grid, T value, int count)
        {
            Fill(grid, value, count, Array.Empty<Coordinate>());
        }
        internal static void Fill<T>(this Grid<T> grid, T value)
        {
            Fill(grid, value, grid.Width * grid.Height);
        }
        internal static void Fill<T>(this Grid<T> grid, T value, int count, Coordinate[] ignoredCoords)
        {
            if (grid.Width == 0 || grid.Height == 0 || count == 0)
            {
                return;
            }

            var coords = grid.GetCoordinates().Except(ignoredCoords).ToArray();

            int maxCycle = coords.Length < count ? coords.Length : count;
            for (int i = 0; i < maxCycle; i++)
            {
                grid[coords[i]] = value;
            }
        }
        internal static void RandomFill<T>(this Grid<T> grid, T[] values)
        {
            RandomFill(grid, values, Array.Empty<Coordinate>());
        }
        internal static void RandomFill<T>(this Grid<T> grid, T value, int count)
        {
            RandomFill(grid, value, count, Array.Empty<Coordinate>());
        }
        internal static void RandomFill<T>(this Grid<T> grid, T[] values, Coordinate[] fixedCoords)
        {
            if ((grid.Width * grid.Height) - fixedCoords.Length < values.Length)
            {
                throw new ArgumentException($"Values size({values.Length}) could not be larger than target coords size({fixedCoords.Length})");
            }
            if (grid.Width == 0 || grid.Height == 0 || values.Length == 0)
            {
                return;
            }

            var rnd = new Random();
            var coords = grid.GetCoordinates().Except(fixedCoords).ToList();
            var valList = values.ToList();
            while (valList.Count > 0 && coords.Count > 0)
            {
                var posID = rnd.Next(0, coords.Count);
                var valID = rnd.Next(0, valList.Count);
                grid[coords[posID]] = valList[valID];
                coords.RemoveAt(posID);
                valList.RemoveAt(valID);
            }
        }
        internal static void RandomFill<T>(this Grid<T> grid, T value, int count, Coordinate[] fixedCoords)
        {
            if ((grid.Width * grid.Height) - fixedCoords.Length < count)
            {
                throw new ArgumentException($"Values size({count}) could not be larger than target coords size({fixedCoords.Length})");
            }
            if (grid.Width == 0 || grid.Height == 0 || count == 0)
            {
                return;
            }

            var rnd = new Random();
            var coordList = grid.GetCoordinates().Except(fixedCoords).ToList();
            for (int i = 0; i < count; i++)
            {
                int posID = rnd.Next(0, coordList.Count);
                grid[coordList[posID]] = value;
                coordList.RemoveAt(posID);
            }
        }
        internal static void Shuffle<T>(this Grid<T> grid)
        {
            Shuffle(grid, Array.Empty<Coordinate>());
        }
        internal static void Shuffle<T>(this Grid<T> grid, Coordinate[] fixedCoords)
        {
            var rnd = new Random();
            // 选择可进行随机化的坐标组
            Coordinate[] allowedCoords = grid.GetCoordinates().Except(fixedCoords).ToArray();
            // 打乱除了保护坐标外的其他坐标序列，算法为洗牌算法
            for (int i = 0; i < allowedCoords.Length; i++)
            {
                var coord1 = allowedCoords[i];
                var coord2 = allowedCoords[rnd.Next(i, allowedCoords.Length)];
                (grid[coord2], grid[coord1]) = (grid[coord1], grid[coord2]);
            }
        }
        internal static string ToString<T>(this Grid<T> grid, string? format)
        {
            string lArgs = format?.ToLower() ?? "";
            bool align = lArgs.Contains('a');
            bool matrixStyle = lArgs.Contains('m');
            int width = grid.Width;
            int height = grid.Height;

            int maxCellLen = grid.Select(c => c?.ToString()?.Length ?? 0).Max();
            var sb = new StringBuilder();
            if (matrixStyle)
            {
                for (int y = 0; y < height; y++)
                {
                    sb.Append(CreateRow(y));
                    if (y < height - 1)
                    {
                        sb.AppendLine();
                    }
                }
            }
            else
            {
                for (int y = height - 1; y >= 0; y--)
                {
                    sb.Append(CreateRow(y));
                    if (y > 0)
                    {
                        sb.AppendLine();
                    }
                }
            }

            return sb.ToString();

            string CreateRow(int y)
            {
                var sb = new StringBuilder();
                for (int x = 0; x < width; x++)
                {
                    string cell = grid[x, y]?.ToString() ?? "";
                    if (align && cell.Length < maxCellLen)
                    {
                        cell += new string(' ', maxCellLen - cell.Length);
                    }
                    if (x < width - 1)
                    {
                        cell += ' ';
                    }
                    sb.Append(cell);
                }
                return sb.ToString();
            }
        }
    }
}