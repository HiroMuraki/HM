using System.Reflection;

namespace HM.Data.Database
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class DbFieldAttribute : Attribute
    {
        public string Name { get; }
        public DbFieldMode Mode { get; init; } = DbFieldMode.Normal;

        public DbFieldAttribute(string columnName)
        {
            Name = columnName;
        }

        public static DbFieldInfo[] GetColumns<T>()
        {
            var properties = typeof(T).GetProperties();
            var result = new List<DbFieldInfo>(properties.Length);
            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].GetCustomAttribute<DbFieldAttribute>() is DbFieldAttribute attr)
                {
                    result.Add(new DbFieldInfo()
                    {
                        PropertyName = properties[i].Name,
                        ColumnName = attr.Name,
                        Mode = attr.Mode
                    });
                }
            }
            return result.ToArray();
        }
    }
}