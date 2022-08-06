#nullable enable
using System;

namespace HM.UnityEngine
{
    public interface INotifyValueChanged
    {
        public event EventHandler<ValueChangedEventArgs> ValueChanged;
    }
}
