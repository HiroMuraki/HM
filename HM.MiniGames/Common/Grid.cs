#nullable enable
using HM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace HM.MiniGames.Common
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
                return _items[y * Width + x];
            }
            set
            {
                CheckCoordinate(x, y);
                _items[y * Width + x] = value;
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
        public int Width => _width;
        public int Height => _height;
        public int Count => _width * _height;
        #endregion

        #region Methods
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
        /// <summary>
        /// 检测指定坐标是否合法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="grid"></param>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public bool IsValidCoordinate(Coordinate coordinate)
        {
            return IsValidCoordinate(coordinate.X, coordinate.Y);
        }
        /// <summary>
        /// 检测指定坐标是否合法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="grid"></param>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public bool IsValidCoordinate(int x, int y)
        {
            return x >= 0
                && x < Width
                && y >= 0
                && y < Height;
        }
        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            var sb = new StringBuilder();
            for (int y = Height - 1; y >= 0; y--)
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
                if (y != 0)
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
            return Height << 2 ^ Width;
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
                    T a = this[x, y];
                    T b = other[x, y];
                    if (a is IEquatable<T> equatableA && !equatableA.Equals(b))
                    {
                        return false;
                    }
                    else if (b is IEquatable<T> equatableB && !equatableB.Equals(a))
                    {
                        return false;
                    }
                    else if (!Equals(a, b))
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
            _width = width;
            _height = height;
            _items = new T[_width * _height];
        }
        private readonly T[] _items;
        private readonly int _width;
        private readonly int _height;
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