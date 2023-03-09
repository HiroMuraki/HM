#nullable enable
using HM;
using System;

namespace HM.UnityEngine.Scripts.HM
{
    public class ValueChangedEventArgs : EventArgs
    {
        public string? PropertyName { get; set; }
        public object? Value { get; set; }
    }
}
