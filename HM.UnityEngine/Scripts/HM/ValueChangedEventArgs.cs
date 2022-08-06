#nullable enable
using System;

namespace HM.UnityEngine
{
    public class ValueChangedEventArgs : EventArgs
    {
        public string? PropertyName { get; set; }
        public object? Value { get; set; }
    }
}
