namespace HM.Debug
{
    [Flags]
    public enum CharTypes
    {
        #pragma warning disable format
        None         = 0b0000_0000,
        UpperLetters = 0b0000_0001,
        LowerLetters = 0b0000_0010,
        Letters      = UpperLetters | LowerLetters,
        Digits       = 0b0000_0100,
        Symbols      = 0b0000_1000,
        All          = 0b0000_1111
        #pragma warning restore format
    };
}
