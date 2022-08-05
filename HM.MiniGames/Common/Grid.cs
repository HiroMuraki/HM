using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;

namespace HM.MiniGames
{
    /// <summary>
    /// 二维坐标网格，该网格以左下角为坐标原点，向上为y轴正方向，向右为x轴正方向（参考直角坐标系）
    ///  Y -
    ///   |
    ///   |
    ///   |
    /// (0,0) ____________ X+
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class Grid<T> : IEnumerable<T>, IEnumerable, IEquatable<Grid<T>>, IFormattable
    {
        public static readonly Grid<T> Empty = new(0, 0);

        #region Properties
        public T this[int x, int y]
        {
            get
            {
                CheckCoordinate(x, y);
                return _origin2DArray[y, x];
            }
            set
            {
                if (_locked)
                {
                    throw new InvalidOperationException("Unable to modify locked grid");
                }
                CheckCoordinate(x, y);
                _origin2DArray[y, x] = value;
            }
        }
        public T this[Coordinate coord]
        {
            get
            {
                return this[coord.X, coord.Y];
            }
            set
            {
                this[coord.X, coord.Y] = value;
            }
        }
        public int Height => _origin2DArray.GetLength(0);
        public int Width => _origin2DArray.GetLength(1);
        #endregion

        #region Methods
        /// <summary>
        /// 锁定网格，状态变为只读
        /// </summary>
        /// <returns></returns>
        public Grid<T> Lock()
        {
            _locked = true;
            return this;
        }
        /// <summary>
        /// 解除对网格的锁定，取消只读状态
        /// </summary>
        /// <returns></returns>
        public Grid<T> Unlock()
        {
            _locked = false;
            return this;
        }
        /// <summary>
        /// 查找网格中符合条件的元素的坐标
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Coordinate[] FindCoordinates(Predicate<T> predicate)
        {
            var result = new List<Coordinate>();
            foreach (var coordiante in GetCoordinates())
            {
                result.Add(coordiante);
            }
            return result.ToArray();
        }
        /// <summary>
        /// 查找网格中符合条件的元素的坐标
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryFindCoordinates(Predicate<T> predicate, out Coordinate[] result)
        {
            result = FindCoordinates(predicate);
            return result.Length != 0;
        }
        /// <summary>
        /// 获取网格的坐标（从左到右，从下到上）
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Coordinate> GetCoordinates()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    yield return new Coordinate(x, y);
                }
            }
        }
        /// <summary>
        /// 获取所有元素
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in GetCoordinates())
            {
                yield return this[item.X, item.Y];
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            var sb = new StringBuilder();
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (this[x, y] is IFormattable formattable)
                    {
                        sb.Append(formattable.ToString(format, formatProvider));
                    }
                    else
                    {
                        sb.Append(this[x, y]);
                    }
                    if (x != Width - 1)
                    {
                        sb.Append(' ');
                    }
                }
                if (y != Height - 1)
                {
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }
        public string ToString(string? format)
        {
            return ToString(format, null);
        }
        public override string ToString()
        {
            return ToString("", null);
        }
        public override bool Equals(object? obj)
        {
            return Equals(obj as Grid<T>);
        }
        public override int GetHashCode()
        {
            return (Height << 2) ^ Width;
        }
        public bool Equals(Grid<T>? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            // 先比较网格宽度和高度是否相同，再比较元素
            if (Height != other.Height || Width != other.Width)
            {
                return false;
            }
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    T a = _origin2DArray[y, x];
                    T b = other._origin2DArray[y, x];
                    if (a is IEquatable<T> equatableA && !equatableA.Equals(b))
                    {
                        return false;
                    }
                    else if (b is IEquatable<T> equatableB && !equatableB.Equals(a))
                    {
                        return false;
                    }
                    else if (Equals(_origin2DArray[y, x], other._origin2DArray[x, y]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        #endregion

        public static Grid<T> Create(int width, int height)
        {
            return new Grid<T>(width, height);
        }

        private Grid(int width, int height)
        {
            _origin2DArray = new T[height, width];
        }
        private bool _locked;
        private readonly T[,] _origin2DArray;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckCoordinate(int x, int y)
        {
            if (y < 0 || y >= Height || x < 0 || x >= Width)
            {
                throw new CoordinateOutOfRangeException(new Coordinate(x, y));
            }
        }
    }
}