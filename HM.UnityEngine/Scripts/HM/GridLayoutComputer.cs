#nullable enable
using UnityEngine;
using System;

namespace HM.UnityEngine
{
    public class GridLayoutComputer
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public Vector2 Center { get; set; }
        public Vector2 DesiredLayoutSize { get; set; }
        public Vector2 DesiredCellSize { get; set; }
        public Vector2 DesiredMarginPerCell { get; set; }

        public GridLayoutInfo GetGridLayoutInfo(GridLayoutScaleMode scaleMode)
        {
            if (scaleMode == GridLayoutScaleMode.NoScale)
            {
                return GetGridLayoutInfoCore(Rows, Columns, Center, DesiredCellSize, DesiredMarginPerCell);
            }
            else if (scaleMode == GridLayoutScaleMode.AutoScale)
            {
                return GetAutoScaledGridLayoutInfo();
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(scaleMode));
            }
        }
        public GridLayoutInfo GetAutoScaledGridLayoutInfo()
        {
            var actualLayoutSize = new Vector2
            {
                x = (DesiredCellSize.x * Columns) + (Columns - 1) * DesiredMarginPerCell.x,
                y = (DesiredCellSize.y * Rows) + (Rows - 1) * DesiredMarginPerCell.y,
            };
            if (actualLayoutSize.x < DesiredLayoutSize.x && actualLayoutSize.y < DesiredLayoutSize.y)
            {
                return GetGridLayoutInfoCore(Rows, Columns, Center, DesiredCellSize, DesiredMarginPerCell);
            }
            else
            {
                float widthScaleRatio = DesiredLayoutSize.x / actualLayoutSize.x;
                float heightScaleRatio = DesiredLayoutSize.y / actualLayoutSize.y;
                float scaleRatio;
                if (widthScaleRatio > heightScaleRatio)
                {
                    scaleRatio = heightScaleRatio;
                }
                else
                {
                    scaleRatio = widthScaleRatio;
                }
                var cellSize = scaleRatio * DesiredCellSize;
                var marginPerCell = scaleRatio * DesiredMarginPerCell;
                return GetGridLayoutInfoCore(Rows, Columns, Center, cellSize, marginPerCell);
            }
        }

        private static GridLayoutInfo GetGridLayoutInfoCore(
            int rows,
            int columns,
            Vector2 center,
            Vector2 cellSize,
            Vector2 marginPerCell)
        {
            var info = new GridLayoutInfo(rows, columns, cellSize);
            var layoutSize = new Vector2
            {
                x = columns * cellSize.x + (columns - 1) * marginPerCell.x,
                y = rows * cellSize.y + (rows - 1) * marginPerCell.y
            };
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    info[x, y] =
                        center
                        - layoutSize / 2
                        + cellSize / 2
                        + new Vector2
                        {
                            x = cellSize.x * x + x * marginPerCell.x,
                            y = cellSize.y * y + y * marginPerCell.y
                        };
                }
            }
            return info;
        }
    }
}
