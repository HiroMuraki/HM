using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HM.Wpf {
    public interface ISelectable {
        bool IsSelected { get; set; }
    }


    public static class DataCarrier {
        /// <summary>
        /// comment
        /// </summary>
        public static readonly DependencyProperty AttatchedDataProperty
           = DependencyProperty.RegisterAttached("AttatchedData", typeof(object), typeof(DataCarrier));

        public static object GetAttatchedData(DependencyObject obj) {
            return obj.GetValue(AttatchedDataProperty) as object;
        }
        public static void SetAttatchedData(DependencyObject obj, object value) {
            obj.SetValue(AttatchedDataProperty, value);
        }
    }

    public abstract class SelectorList<TControl> : ItemsControl
        where TControl : FrameworkElement, ISelectable {

        protected override async void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e) {
            base.OnPreviewMouseLeftButtonDown(e);

            double heldTime = await Util.GetMouseButtonHeldTime(e.MouseDevice, 110);
            if (heldTime > 100) {
                // 获取目标控件
                var mousePos = e.GetPosition(this);
                var element = VisualTreeUtil.FindVisualParent<TControl>(InputHitTest(mousePos) as DependencyObject);
                if (element is null) {
                    return;
                }
                var tData = element.GetValue(DataCarrier.AttatchedDataProperty);
                if (tData is null) {
                    return;
                }
                DragDrop.DoDragDrop(element, tData, DragDropEffects.Move);
                element.IsSelected = true;
            }
        }
        protected override void OnDrop(DragEventArgs e) {
            base.OnDrop(e);
        }
        protected override void OnDragOver(DragEventArgs e) {
            base.OnDragOver(e);
        }
        protected override void OnDragLeave(DragEventArgs e) {
            base.OnDragLeave(e);
        }

    }
}
