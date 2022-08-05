#nullable enable

namespace HM.UnityEngine
{
    public class LayoutComputer
    {
        /// <summary>
        /// 网格的行数
        /// </summary>
        public int Rows { get; set; }
        /// <summary>
        /// 网格的列数
        /// </summary>
        public int Columns { get; set; }
        /// <summary>
        /// 每个单元格的大小
        /// </summary>
        public Vector2 CellSize { get; set; }
        /// <summary>
        /// 中心点
        /// </summary>
        public Vector2 Center { get; set; }
        /// <summary>
        /// 每个单元格之间的边距
        /// </summary>
        public Vector2 Margin { get; set; }
        /// <summary>
        /// 当前设置下预期的网格大小
        /// </summary>
        public Vector2 LayoutSize
        {
            get
            {
                return new Vector2(Columns * CellSize.x, Rows * CellSize.y)
                    + new Vector2(Margin.x * (Columns - 1), Margin.y * (Rows - 1));
            }
        }

        public Vector2 ComputeCellPosition(int x, int y)
        {
            return new Vector2((CellSize.x + Margin.x) * x, (CellSize.y + Margin.y) * y)
                + CellSize / 2
                - LayoutSize / 2
                + Center;
        }
    }
}