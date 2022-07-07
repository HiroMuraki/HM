using System.Collections;
using System.Reflection;

namespace HM.Debug
{
    public static class ObjectInsepctor
    {
        private class PropertyInfo
        {
            public string Name { get; set; } = "";
            public string Type { get; set; } = "";
            public List<string> Values { get; set; } = new();
            public int TotalStringLengh => Name.Length + Type.Length + (from i in Values select i.Length).Sum();
            public override string ToString()
            {
                return $"[{Type}]{Name}: {string.Join(", ", Values)}";
            }
        }

        public static Action<string> Printer { get; set; } = Console.WriteLine;

        public static void PrintProperties(object obj)
        {
            Printer(GetFormattedProperties(obj, _defaultBindingFlags));
        }
        public static void PrintProperties(object obj, BindingFlags flag)
        {
            Printer(GetFormattedProperties(obj, flag));
        }
        public static string GetFormattedProperties(object obj)
        {
            return GetFormattedProperties(obj, _defaultBindingFlags);
        }
        public static string GetFormattedProperties(object obj, BindingFlags flag)
        {
            string hName = "[H]-Name";
            string hType = "[H]-Type";
            string hValue = "[H]-Value";
            var propertiesInfo = GetPropertiesInfo(obj, flag);
            var tableBuilder = new TableBuilder()
            {
                Padding = 3
            };
            tableBuilder.Headers = new string[] { hName, hValue, hType };
            foreach (var propertyInfo in propertiesInfo)
            {
                if (propertyInfo.Values.Count >= 2)
                {
                    // 首行正常处理，从第二行开始，仅打印值，Name和Type留空
                    tableBuilder.Rows.Add(new string[] { propertyInfo.Name, propertyInfo.Values[0], propertyInfo.Type });
                    for (int i = 1; i < propertyInfo.Values.Count; i++)
                    {
                        tableBuilder.Rows.Add(new string[] { "", propertyInfo.Values[i], "" });
                    }
                }
                else
                {
                    tableBuilder.Rows.Add(new string[] { propertyInfo.Name, propertyInfo.Values[0], propertyInfo.Type });
                }
            }
            return tableBuilder.ToString(tableBuilder.GetColumnsWidths(), CellAlignment.Left);
        }

        private static readonly BindingFlags _defaultBindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
        private static PropertyInfo[] GetPropertiesInfo(object obj)
        {
            return GetPropertiesInfo(obj, _defaultBindingFlags);
        }
        private static PropertyInfo[] GetPropertiesInfo(object obj, BindingFlags flag)
        {
            var propertiesInfo = new List<PropertyInfo>();
            foreach (var p in obj.GetType().GetProperties(flag))
            {
                var propertyInfo = new PropertyInfo();
                propertyInfo.Name = p.Name;
                propertyInfo.Type = p.PropertyType.Name;
                // 特殊对待集合
                if (p.PropertyType.GetInterface(nameof(ICollection)) is not null)
                {
                    var enumerator = p.GetValue(obj) as IEnumerable;
                    if (enumerator is null)
                    {
                        propertyInfo.Values.Add("null");
                    }
                    else
                    {
                        int index = 0;
                        foreach (var item in enumerator)
                        {
                            propertyInfo.Values.Add($"[{index}] = {item}");
                            index++;
                        }
                    }
                }
                // 特殊对待Item属性，这可能是索引器
                else if (p.Name == "Item")
                {
                    propertyInfo.Values.Add("Indexer[]");
                }
                // 其他情况正常获取值即可
                else
                {
                    propertyInfo.Values.Add(p.GetValue(obj)?.ToString() ?? "null");
                }
                propertiesInfo.Add(propertyInfo);
            }
            return propertiesInfo.ToArray();
        }
    }
}
