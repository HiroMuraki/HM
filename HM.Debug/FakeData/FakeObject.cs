#pragma warning disable IDE0049

using System.Reflection;
using System.Text;

namespace HM.Debug.FakeData
{
    public abstract class FakeObject
    {
        public override string ToString()
        {
            PropertyInfo[] propInfos = GetPropertyInfosFromCache();

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

            PropertyInfo[] propInfos = GetPropertyInfosFromCache();
            foreach (PropertyInfo propInfo in propInfos)
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

        private static readonly Dictionary<Type, PropertyInfo[]> _propertyInfosCache = new();
        private PropertyInfo[] GetPropertyInfosFromCache()
        {
            Type type = GetType();
            if (_propertyInfosCache.TryGetValue(type, out PropertyInfo[]? propInfos))
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
