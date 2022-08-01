using System.Reflection;

namespace HM.Common.Extensions
{
    public static class MemberInfoExtension
    {
        public static bool TryGetAttribute<T>(this MemberInfo self, out T? result) where T : Attribute
        {
            result = Attribute.GetCustomAttribute(self, typeof(T)) as T;
            return result != null;
        }
        public static bool TryGetAttributes<T>(this MemberInfo self, out T[]? result) where T : Attribute
        {
            result = Attribute.GetCustomAttributes(self, typeof(T)) as T[];
            return result != null;
        }
        public static bool HasAttribute<T>(this MemberInfo self) where T : Attribute
        {
            return Attribute.IsDefined(self, typeof(T));
        }
    }
}
