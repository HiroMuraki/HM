#pragma warning disable IDE0049

using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Schema;

namespace HM.Debug.FakeData
{
    public abstract class FakeObject<T>
        where T : new()
    {
        public override string ToString()
        {
            var propInfos = GetPropertyInfosFromCache();

            var sb = new StringBuilder();
            sb.Append('{');
            for (int i = 0; i < propInfos!.Length; i++)
            {
                sb.Append($"{propInfos[i].Name} = {propInfos[i].GetValue(this)}");
                if (i < propInfos.Length - 1)
                {
                    sb.Append(", ");
                }
            }
            sb.Append('}');
            return sb.ToString();
        }
        public override bool Equals(object? obj)
        {
            if (obj == this) return true;
            if (obj is null) return false;
            if (obj.GetType() != GetType()) return false;

            var propInfos = GetPropertyInfosFromCache();
            foreach (var propInfo in propInfos)
            {
                object? valA = propInfo.GetValue(this);
                object? valB = propInfo.GetValue(obj);
                if (valA is null && valB is null)
                    continue;
                if (valA is null && valB is not null)
                    return false;
                if (valA is not null && valB is null)
                    return false;
                if (valA is not null && valB is not null && !valA.Equals(valB))
                    return false;
            }

            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static T FakeOne() => FakeOne(null);
        public static T FakeOne(Action<T>? setter)
        {
            var result = new T();
            setter?.Invoke(result);
            return result;
        }
        public static T[] FakeMany(int count) => FakeMany(count, null);
        public static T[] FakeMany(int count, Action<T>? setter = null)
        {
            var result = new List<T>();
            for (int i = 0; i < count; i++)
            {
                result.Add(FakeOne(setter));
            }
            return result.ToArray();
        }

        private static readonly Dictionary<Type, PropertyInfo[]> _propertyInfosCache = new();
        private PropertyInfo[] GetPropertyInfosFromCache()
        {
            var type = GetType();
            if (_propertyInfosCache.TryGetValue(type, out var propInfos))
            {
                return propInfos;
            }
            else
            {
                propInfos = type.GetProperties().ToArray();
                _propertyInfosCache[type] = propInfos;
                return propInfos;
            }
        }
    }
}
