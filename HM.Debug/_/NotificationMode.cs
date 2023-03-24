namespace HM.Debug._d
{
    [Flags]
    public enum NotificationMode
    {
        #pragma warning disable format
        None    = 0b_0000_0000,
        Preview = 0b_0000_0001,
        Normal  = 0b_0000_0010,
        All     = 0b_0000_0011,
        #pragma warning restore format
    }
}
