#nullable enable
using HM;
using HM.UnityEngine._FakeUnityAPI;

namespace HM.UnityEngine.Scripts.HM
{
    public class GridLayoutInfo
    {
        public int Rows { get; }
        public int Columns { get; }
        public Vector2 CellSize { get; }

        public Vector2 this[int x, int y]
        {
            get
            {
                return _array[y * Columns + x];
            }
            set
            {
                _array[y * Columns + x] = value;
            }
        }

        public GridLayoutInfo(int rows, int columns, Vector2 cellSize)
        {
            Rows = rows;
            Columns = columns;
            CellSize = cellSize;
            _array = new Vector2[Rows * Columns];
        }

        private readonly Vector2[] _array;
    }
}
