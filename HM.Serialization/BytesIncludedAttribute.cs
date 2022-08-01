namespace HM.Serialization
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class BytesIncludedAttribute : Attribute
    {
        public int Order { get; }

        public BytesIncludedAttribute(int order)
        {
            Order = order;
        }
    }
}
