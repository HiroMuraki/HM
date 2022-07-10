using System.Text;

namespace HM.Collections.Extensions
{
    public static class MDArrayExtension
    {
        public static string ToFormattedString<T>(this T?[,] self, string? format = null)
        {
            string lArgs = format?.ToLower() ?? "";
            bool coordStyle = lArgs.Contains('c');
            bool alignL = lArgs.Contains('l');
            bool alignR = lArgs.Contains('r');
            int rowSize = self.GetLength(0);
            int columnSize = self.GetLength(1);

            int maxCellLen = (from i in self.ToEnumerable()
                              select i.ToString()?.Length ?? 0)
                              .Max();

            var sb = new StringBuilder();
            if (coordStyle)
            {
                for (int y = rowSize - 1; y >= 0; y--)
                {
                    sb.Append(CreateRow(y));
                    if (y > 0)
                    {
                        sb.AppendLine();
                    }
                }
            }
            else
            {
                for (int y = 0; y < rowSize; y++)
                {
                    sb.Append(CreateRow(y));
                    if (y < rowSize - 1)
                    {
                        sb.AppendLine();
                    }
                }
            }

            return sb.ToString();

            string CreateRow(int y)
            {
                var sb = new StringBuilder();
                for (int x = 0; x < columnSize; x++)
                {
                    string cell = self[y, x]?.ToString() ?? "";
                    if (alignL && alignR)
                    {
                        int spaceLen = maxCellLen - cell.Length;
                        cell = cell.PadLeft(cell.Length + spaceLen / 2);
                        cell = cell.PadRight(cell.Length + (spaceLen + 1) / 2);
                    }
                    else if (alignL)
                    {
                        cell = cell.PadRight(maxCellLen);
                    }
                    else if (alignR)
                    {
                        cell = cell.PadLeft(maxCellLen);
                    }
                    if (x < columnSize - 1)
                    {
                        cell += ' ';
                    }
                    sb.Append(cell);
                }
                return sb.ToString();
            }
        }
        public static IEnumerable<T?> ToEnumerable<T>(this T?[,] self)
        {
            foreach (var item in self)
            {
                yield return item;
            }
        }
        public static T?[,] Shrink<T>(this T?[,] self, int size)
        {
            return self.Shrink(size, size, size, size);
        }
        public static T?[,] Shrink<T>(this T?[,] self, int left, int up, int right, int down)
        {
            if (left < 0 || up < 0 || right < 0 || down < 0)
            {
                throw new ArgumentOutOfRangeException("shrink size should be larger than zero");
            }
            int newRowSize = self.GetLength(0) - (up + down);
            int newColumnSize = self.GetLength(1) - (left + right);
            int offsetY = up;
            int offsetX = left;

            if (newRowSize <= 0 || newColumnSize <= 0)
            {
                return new T[0, 0];
            }

            var newMArray = new T?[newRowSize, newColumnSize];

            for (int y = 0; y < newRowSize; y++)
            {
                for (int x = 0; x < newColumnSize; x++)
                {
                    newMArray[y, x] = self[y + offsetY, x + offsetX];
                }
            }

            return newMArray;
        }
        public static T?[,] Expand<T>(this T?[,] self, int size, T? fillValue = default)
        {
            return self.Expand(size, size, size, size, fillValue);
        }
        public static T?[,] Expand<T>(this T?[,] self, int left, int up, int right, int down, T? fillValue = default)
        {
            if (left < 0 || up < 0 || right < 0 || down < 0)
            {
                throw new ArgumentOutOfRangeException("expand size should be larger than zero");
            }

            int newRowSize = self.GetLength(0) + up + down;
            int newColumnSize = self.GetLength(1) + left + right;
            int offsetX = left;
            int offsetY = up;

            var newMArray = new T?[newRowSize, newColumnSize];
            if (!Equals(fillValue, default(T)))
            {
                for (int y = 0; y < up; y++)
                {
                    for (int x = 0; x < newColumnSize; x++)
                    {
                        newMArray[y, x] = fillValue;
                    }
                }
                for (int y = 0; y < down; y++)
                {
                    for (int x = 0; x < newColumnSize; x++)
                    {
                        newMArray[newRowSize - 1 - y, x] = fillValue;
                    }
                }
                for (int x = 0; x < left; x++)
                {
                    for (int y = 0; y < newRowSize; y++)
                    {
                        newMArray[y, x] = fillValue!;
                    }
                }
                for (int x = 0; x < right; x++)
                {
                    for (int y = 0; y < newRowSize; y++)
                    {
                        newMArray[y, newColumnSize - 1 - x] = fillValue;
                    }
                }
            }

            for (int y = 0; y < self.GetLength(0); y++)
            {
                for (int x = 0; x < self.GetLength(1); x++)
                {
                    newMArray[y + offsetY, x + offsetX] = self[y, x];
                }
            }

            return newMArray;
        }
        public static void Fill<T>(this T?[,] self, T? value)
        {
            for (int row = 0; row < self.GetLength(0); row++)
            {
                for (int col = 0; col < self.GetLength(1); col++)
                {
                    self[row, col] = value;
                }
            }
        }
        public static void Fill<T>(this T?[,] self, Func<T> valueIter)
        {
            for (int row = 0; row < self.GetLength(0); row++)
            {
                for (int col = 0; col < self.GetLength(1); col++)
                {
                    self[row, col] = valueIter();
                }
            }
        }
        public static void Transpose<T>(this T?[,] self)
        {
            if (self.GetLength(0) != self.GetLength(1))
            {
                throw new NotSupportedException("Row size should equals column size");
            }
            int sideLen = self.GetLength(0);
            for (int row = 0; row < sideLen; row++)
            {
                for (int col = row + 1; col < sideLen; col++)
                {
                    var t = self[row, col];
                    self[row, col] = self[col, row];
                    self[col, row] = t;
                }
            }
        }
        public static T?[,] ToTransposed<T>(this T?[,] self)
        {
            var transposed = new T?[self.GetLength(1), self.GetLength(0)];
            for (int tRow = 0; tRow < self.GetLength(1); tRow++)
            {
                for (int tCol = 0; tCol < self.GetLength(0); tCol++)
                {
                    transposed[tRow, tCol] = self[tCol, tRow];
                }
            }
            return transposed;
        }
        public static IEnumerable<T?[]> TakeRows<T>(this T?[,] self)
        {
            for (int row = 0; row < self.GetLength(0); row++)
            {
                var rowContent = new T?[self.GetLength(1)];
                for (int col = 0; col < self.GetLength(1); col++)
                {
                    rowContent[col] = self[row, col];
                }
                yield return rowContent;
            }
        }
        public static IEnumerable<T?[]> TakeColumns<T>(this T?[,] self)
        {
            for (int col = 0; col < self.GetLength(1); col++)
            {
                var colContent = new T?[self.GetLength(0)];
                for (int row = 0; row < self.GetLength(0); row++)
                {
                    colContent[row] = self[row, col];
                }
                yield return colContent;
            }
        }
        public static T?[][] ToJigsawArray<T>(this T?[,] self)
        {
            var result = new T?[self.GetLength(0)][];
            for (int row = 0; row < result.Length; row++)
            {
                result[row] = new T?[self.GetLength(1)];
                for (int col = 0; col < self.GetLength(1); col++)
                {
                    result[row][col] = self[row, col];
                }
            }
            return result;
        }
    }
}
