using System.Reflection;
using System.Text;

namespace HM.Debug
{
    public static class ObjectExtension
    {
        public static void WriteSelf(this object self, string title = "", Action<string?>? writer = null)
        {
            if (!string.IsNullOrEmpty(title))
            {
                writer?.Invoke(title);
            }

            writer?.Invoke(self?.ToString());
        }
        public static T[]? GetMembers<T>(this object? obj, BindingFlags bindingAttr) where T : MemberInfo
        {
            if (obj is null)
            {
                return null;
            }
            var memberInfos = (from i in obj.GetType().GetMembers(bindingAttr)
                               where i is T
                               select (T)i).ToArray();
            var pairs = new T[memberInfos.Length];
            var valueGetter = typeof(T).GetMethod("GetValue", new Type[] { typeof(object) });
            for (int i = 0; i < memberInfos.Length; i++)
            {
                pairs[i] = memberInfos[i];
            }
            return pairs;
        }
        public static T[]? GetMembers<T>(this object? obj) where T : MemberInfo
        {
            return obj?.GetMembers<T>(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }
        public static string? ToKeyValuePairs(this object? obj)
        {
            if (obj is null)
            {
                return null;
            }
            var sb = new StringBuilder();
            sb.Append('{');
            var properties = obj.GetType().GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                string name = properties[i].Name;
                object? value = properties[i].GetValue(obj);
                bool valueRequiresBracket = true;
                switch (Type.GetTypeCode(value?.GetType()))
                {
                    case TypeCode.Empty:
                    case TypeCode.Char:
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                    case TypeCode.Boolean:
                        valueRequiresBracket = false;
                        break;
                    case TypeCode.Object:
                    case TypeCode.DBNull:
                    case TypeCode.DateTime:
                    case TypeCode.String:
                        valueRequiresBracket = true;
                        break;
                }
                if (valueRequiresBracket)
                {
                    sb.Append($"\"{name}\":\"{value}\"");
                }
                else
                {
                    sb.Append($"\"{name}\":{value}");
                }
                if (i < properties.Length - 1)
                {
                    sb.Append(',');
                }
            }
            sb.Append('}');
            return sb.ToString();
        }
    }
}
