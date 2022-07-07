using System.Text;

namespace HM.Debug
{
    public class TableBuilder
    {
        public string[]? Headers { get; set; } = Array.Empty<string>();
        public IList<string[]> Rows { get; } = new List<string[]>();
        public bool IncludeHeaders { get; set; } = true;
        public char HorizontalTAB { get; set; } = '-';
        public char VerticalTAB { get; set; } = '|';
        public int Padding { get; set; }

        public IEnumerable<string> GetFormattedRows()
        {
            return GetFormattedRows(GetColumnsWidths(), CellAlignment.Left);
        }
        public IEnumerable<string> GetFormattedRows(int[] columnWidths, CellAlignment cellAlignment)
        {
            CheckTable();
            if (!IncludeHeaders && Rows.Count == 0)
            {
                yield break;
            }
            else if (IncludeHeaders && Headers is null)
            {
                throw new ArgumentException("Require Headers but Headers is null");
            }

            if (IncludeHeaders)
            {
                yield return string.Join(VerticalTAB, FormatRow(Headers!, columnWidths, Padding, cellAlignment));
            }
            foreach (var row in Rows)
            {
                yield return string.Join(VerticalTAB, FormatRow(row, columnWidths, Padding, cellAlignment));
            }
        }
        public int[] GetColumnsWidths()
        {
            return GetColumnsWidths(null);
        }
        public int[] GetColumnsWidths(Func<int, int>? increaseAdditionalWidth)
        {
            CheckTable();
            int columns = IncludeHeaders ? Headers!.Length : Rows[0].Length;
            int[] columnsLenInfo = new int[columns];
            for (int i = 0; i < columnsLenInfo.Length; i++)
            {
                int headerLen = IncludeHeaders ? Headers![i].Length : 0;
                int maxColLen = (from row in Rows select row[i].Length).Max();
                columnsLenInfo[i] = headerLen > maxColLen ? headerLen : maxColLen;
                if (increaseAdditionalWidth is not null)
                {
                    columnsLenInfo[i] += increaseAdditionalWidth(i);
                }
                columnsLenInfo[i] += Padding * 2;
            }
            return columnsLenInfo;
        }
        public override string ToString()
        {
            return ToString(GetColumnsWidths(), CellAlignment.Left);
        }
        public string ToString(int[] columnWidths, CellAlignment cellAlignment)
        {
            var sb = new StringBuilder();
            string rowLine = new string(HorizontalTAB, columnWidths.Sum() + columnWidths.Length - 1);
            foreach (var row in GetFormattedRows(columnWidths, cellAlignment))
            {
                sb.AppendLine(row);
                sb.AppendLine(rowLine);
            }
            return sb.ToString();
        }

        private void CheckTable()
        {
            int columns = IncludeHeaders ? Headers!.Length : Rows[0].Length;
            for (int i = 0; i < Rows.Count; i++)
            {
                if (Rows[i].Length != columns)
                {
                    throw new ArgumentException($"All the rows in the table should be the same size({columns}), check at row[{i}]");
                }
            }
        }
        private static IEnumerable<string> FormatRow(string[] cells, int[] widths, int padding, CellAlignment cellAlignment)
        {
            string pad = new string(' ', padding);
            for (int i = 0; i < cells.Length; i++)
            {
                string current = pad + cells[i] + pad;
                switch (cellAlignment)
                {
                    default:
                    case CellAlignment.Left:
                        current = current.PadRight(widths[i]);
                        break;
                    case CellAlignment.Right:
                        current = current.PadLeft(widths[i]);
                        break;
                    case CellAlignment.Center:
                        // 至中计算：获取cell字符串长度，然后向右偏移与当前列宽度的差值/2，然后在右侧补充
                        int leftOffset = (widths[i] - current.Length) / 2;
                        if (leftOffset < 0)
                        {
                            leftOffset = 0;
                        }
                        current = current.PadLeft(leftOffset + current.Length).PadRight(widths[i]);
                        break;
                }
                yield return current;
            }
        }
    }
}
