using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace HM.Debug.Extensions
{
    public static class ObjectExtension
    {
        public static BindingFlags PublicBindingFlags { get; } = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
        public static BindingFlags NonPublicBindingFlags { get; } = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;
        public static Action<string?> Writer { get; set; } = Console.WriteLine;

        public static string AsJson(this object? obj)
        {
            return JsonSerializer.Serialize(obj, new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            });
        }
        public static void DumpJson(this object? obj) => obj.AsJson().WriteSelf();
        public static void DumpProperties(this object self)
        {
            string.Join('\n', GetMemberLines(self, 4)).WriteSelf();

            static string[] GetMemberLines(object? obj, int indent)
            {
                if (obj is null) return Array.Empty<string>();

                var propertyInfos = obj.GetType().GetProperties(PublicBindingFlags);
                var lines = new List<string>();
                string indentStr = new string(' ', indent);

                lines.Add("{");
                foreach (var p in propertyInfos)
                {
                    object? value = p.GetValue(obj);
                    if (p.PropertyType.IsArray)
                    {
                        string header = $"{indentStr}{p.Name} = [";
                        string headerIndent = new string(' ', header.Length - 1);

                        lines.Add(header);
                        foreach (object? item in (Array)value!)
                        {
                            if (Type.GetTypeCode(item.GetType()) == TypeCode.Object)
                            {
                                string[] itemLines = GetMemberLines(item, indent);
                                lines.AddRange(itemLines.Select(t => indentStr + indentStr + t));
                            }
                            else
                            {
                                lines.Add($"{indentStr}{indentStr}{item}");
                            }
                        }
                        lines.Add($"{indentStr}]");
                    }
                    else if (Type.GetTypeCode(p.PropertyType) != TypeCode.Object)
                    {
                        lines.Add($"{indentStr}{p.Name} = {value}");
                    }
                    else
                    {
                        lines.Add($"{indentStr}{p.Name} = {{");
                        string[] memberLines = GetMemberLines(value, indent);
                        lines.AddRange(memberLines.Skip(1).SkipLast(1).Select(t => indentStr + t));
                        lines.Add($"{indentStr}}}");
                    }
                }
                lines.Add("}");

                return lines.ToArray();
            }
        }
        public static void DumpFields(this object self)
        {
            string.Join('\n', GetMemberLines(self, 4)).WriteSelf();

            static string[] GetMemberLines(object? obj, int indent)
            {
                if (obj is null) return Array.Empty<string>();

                var propertyInfos = obj.GetType().GetFields(NonPublicBindingFlags);
                var lines = new List<string>();
                string indentStr = new string(' ', indent);

                lines.Add("{");
                foreach (var p in propertyInfos)
                {
                    object? value = p.GetValue(obj);
                    if (p.FieldType.IsArray)
                    {
                        string header = $"{indentStr}{p.Name} = [";
                        string headerIndent = new string(' ', header.Length - 1);

                        lines.Add(header);
                        foreach (object? item in (Array)value!)
                        {
                            if (Type.GetTypeCode(item.GetType()) == TypeCode.Object)
                            {
                                string[] itemLines = GetMemberLines(item, indent);
                                lines.AddRange(itemLines.Select(t => indentStr + indentStr + t));
                            }
                            else
                            {
                                lines.Add($"{indentStr}{indentStr}{item}");
                            }
                        }
                        lines.Add($"{indentStr}]");
                    }
                    else if (Type.GetTypeCode(p.FieldType) != TypeCode.Object)
                    {
                        lines.Add($"{indentStr}{p.Name} = {value}");
                    }
                    else
                    {
                        lines.Add($"{indentStr}{p.Name} = {{");
                        string[] memberLines = GetMemberLines(value, indent);
                        lines.AddRange(memberLines.Skip(1).SkipLast(1).Select(t => indentStr + t));
                        lines.Add($"{indentStr}}}");
                    }
                }
                lines.Add("}");

                return lines.ToArray();
            }
        }
        public static void WriteSelf(this object self, string title = "", Action<string?>? writer = null)
        {
            writer ??= Writer;

            if (!string.IsNullOrEmpty(title))
            {
                writer?.Invoke(title);
            }

            writer?.Invoke(self?.ToString());
        }
    }
}
