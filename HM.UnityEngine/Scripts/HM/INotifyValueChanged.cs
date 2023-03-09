#nullable enable
using HM;
using System;

namespace HM.UnityEngine.Scripts.HM
{
    public interface INotifyValueChanged
    {
        public event EventHandler<ValueChangedEventArgs> ValueChanged;
    }
}
