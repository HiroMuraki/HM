using System.Net.Http.Headers;
using System.Text;

namespace HM.CommandLine.StringTable
{
    public class StringTableBuilder
    {
        public Table Table => _table;

        public StringTableBuilder SetHeader(params string[] header)
        {
            CheckRowValidation(header, true);

            _table.Header = header;

            return this;
        }
        public StringTableBuilder AddRow(params object?[] values)
        {
            CheckRowValidation(values);

            _table.Rows.Add(values);

            return this;
        }
        public string FormatTable() => FormatTable(StringTableFormatOptions.Default);
        public string FormatTable(StringTableFormatOptions options) => string.Join('\n', EnumerateFormattedRows(options));
        public void Write(Stream stream, StringTableFormatOptions? options = null, Encoding? encoding = null)
        {
            foreach (var line in EnumerateFormattedRows(options ?? StringTableFormatOptions.Default))
            {
                byte[] data = encoding?.GetBytes(line) ?? Encoding.UTF8.GetBytes(line);
                stream.Write(data, 0, data.Length);
            }
        }
        public void Write(TextWriter textWriter, StringTableFormatOptions? options = null)
        {
            foreach (var line in EnumerateFormattedRows(options ?? StringTableFormatOptions.Default))
            {
                textWriter.WriteLine(line);
            }
        }
        public void Write(StringTableFormatOptions? options = null)
        {
            Write(Console.Instance.Out, options);
        }
        public override string ToString() => FormatTable();

        public static StringTableBuilder CreateFrom<T>(IEnumerable<T> enumerable)
        {
            return CreateFromCore(enumerable, null);
        }
        public static StringTableBuilder CreateFrom<T>(IEnumerable<T> enumerable, string[] properties)
        {
            return CreateFromCore(enumerable, properties);
        }

        private readonly Table _table = new();
        private int GetColumnCount()
        {
            if (_table.Header is not null)
            {
                return _table.Header.Length;
            }
            else if (_table.Rows.Count > 0)
            {
                return _table.Rows[0].Length;
            }
            else
            {
                return 0;
            }
        }
        private void CheckRowValidation(object?[] values, bool ignoreHeaderSize = false)
        {
            if (!ignoreHeaderSize && _table.Header is not null)
            {
                if (values.Length != _table.Header.Length)
                    throw new ArgumentException($"Size of {nameof(values)} not equal to size of {nameof(_table.Header)}");
            }
            else if (_table.Rows.Count > 0)
            {
                if (values.Length != _table.Rows[0].Length)
                    throw new ArgumentException($"Size of {nameof(values)} not equal to size of first row");
            }
        }
        private IEnumerable<string> EnumerateFormattedRows(StringTableFormatOptions options)
        {
            ArgumentNullException.ThrowIfNull(options);

            // columnWidthInfos
            int[] columnWidthInfos = new int[GetColumnCount()];
            string[]? header = _table.Header is not null ? GetRenamedHeader() : null;
            var rows = _table.Rows.Select(ConvertValues).ToList();

            if (header is not null)
            {
                UpdateColumnWidthInfos(header);
            }
            foreach (var row in rows)
            {
                UpdateColumnWidthInfos(row);
            }

            // build string
            string[] horizontalLine = columnWidthInfos.Select(c => new string(
                    options.TableGeneralElements.Horizontal,
                    c + options.CellLeftPadding.Length + options.CellRightPadding.Length)).ToArray();

            // head
            yield return GetHeadLine();
            // header
            if (header is not null)
            {
                yield return GetRowLine(header);
                yield return options.TableCrossElements.Left
                    + string.Join(options.TableCrossElements.Center, horizontalLine)
                    + options.TableCrossElements.Right;
            }
            // rows
            foreach (var row in rows)
            {
                yield return GetRowLine(row);
            }
            // tail
            yield return GetTailLine();

            string[] ConvertValues(object?[] values)
            {
                string[] result = new string[values.Length];
                for (int i = 0; i < values.Length; i++)
                {
                    if (values[i] is null)
                    {
                        result[i] = string.Empty;
                        continue;
                    }
                    else if (options.ValueConverters.TryGetValue(values[i]!.GetType(), out var converter))
                    {
                        result[i] = converter(values[i]);
                    }
                    else
                    {
                        result[i] = values[i]!.ToString() ?? string.Empty;
                    }
                }
                return result;
            }
            string GetRowLine(string[] row)
            {
                string[] GetFormattedCells()
                {
                    string[] formattedCells = new string[row.Length];
                    for (int i = 0; i < row.Length; i++)
                    {
                        formattedCells[i] = options.CellAlignment switch
                        {
                            CellAlignment.Right => row[i].PadLeft(columnWidthInfos[i]),
                            CellAlignment.Left or _ => row[i].PadRight(columnWidthInfos[i]),
                        };
                        formattedCells[i] = options.CellLeftPadding + formattedCells[i] + options.CellRightPadding;
                    }
                    return formattedCells;
                }

                return options.TableGeneralElements.Vertical
                    + string.Join(options.TableGeneralElements.Vertical, GetFormattedCells())
                    + options.TableGeneralElements.Vertical;
            }
            string GetHeadLine()
            {
                return options.TableCornerElements.TopLeft
                    + string.Join(options.TableCrossElements.Top, horizontalLine)
                    + options.TableCornerElements.TopRight;
            }
            string GetTailLine()
            {
                return options.TableCornerElements.BottomLeft
                    + string.Join(options.TableCrossElements.Bottom, horizontalLine)
                    + options.TableCornerElements.BottomRight;
            }
            string[] GetRenamedHeader()
            {
                if (_table.Header is null) return Array.Empty<string>();

                string[] renamedHeader = new string[_table.Header.Length];

                for (int i = 0; i < _table.Header.Length; i++)
                {
                    if (options.HeaderConverters.TryGetValue(_table.Header[i], out string? val))
                    {
                        renamedHeader[i] = val;
                    }
                    else
                    {
                        renamedHeader[i] = _table.Header[i];
                    }
                }

                return renamedHeader;
            }
            void UpdateColumnWidthInfos(string?[] values)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    if (values[i] is null)
                        continue;

                    if (columnWidthInfos[i] < values[i]!.Length)
                    {
                        columnWidthInfos[i] = values[i]!.Length;
                    }
                }
            }
        }
        private static StringTableBuilder CreateFromCore<T>(IEnumerable<T> enumerable, string[]? properties)
        {
            var stb = new StringTableBuilder();

            var propInfos = typeof(T).GetProperties()
                .Where(t => properties?.Contains(t.Name) ?? true)
                .ToArray();

            stb.SetHeader(propInfos.Select(t => t.Name)
                .ToArray());

            foreach (var item in enumerable)
            {
                var values = new object?[propInfos.Length];
                for (int i = 0; i < propInfos.Length; i++)
                {
                    values[i] = propInfos[i].GetValue(item);
                }
                stb.AddRow(values);
            }

            return stb;
        }
    }
}
