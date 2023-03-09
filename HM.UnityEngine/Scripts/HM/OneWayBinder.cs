#nullable enable
using System;
using UnityEngine;
using System.Linq;
using HM.UnityEngine._FakeUnityAPI;

namespace HM.UnityEngine.Scripts.HM
{
    public abstract class OneWayBinder : MonoBehaviour
    {
#pragma warning disable CS8618 // 在Start消息中完成判空
#pragma warning disable CS0649 // 在编辑器完成赋值
        [SerializeField] GameObject gameObjectOrigin;
        [SerializeField] string bindingComponentType;
        [SerializeField] string bindingComponentProperty;
#pragma warning restore CS8618
#pragma warning restore CS0649 

        protected void Initialize()
        {
            if (gameObjectOrigin is null)
            {
                throw new ArgumentException("gameObjectOrigin can't be null");
            }
            if (string.IsNullOrEmpty(bindingComponentType))
            {
                throw new ArgumentException(nameof(bindingComponentProperty) + " is required");
            }
            if (string.IsNullOrEmpty(bindingComponentType))
            {
                throw new ArgumentException(nameof(bindingComponentProperty) + " is required");
            }

            var dataSource = gameObjectOrigin
                .GetComponents<INotifyValueChanged>()
                .First(s => s.GetType().Name == bindingComponentType);
            if (dataSource is null)
            {
                throw new ArgumentException($"Unable to find {gameObjectOrigin}/{bindingComponentProperty}/{bindingComponentProperty}");
            }

            dataSource.ValueChanged += (sender, e) =>
            {
                if (e.PropertyName != bindingComponentProperty)
                {
                    return;
                }
                SetValue(e.Value);
            };
        }
        protected abstract void SetValue(object? value);
    }
}
