using System.Security.Cryptography;
using System.IO;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;

namespace HM.Wpf {
    public static class Util {
        public static async Task<int> GetMouseButtonHeldTime(MouseDevice mouseDevice, int timeOut) {
            int delayMS = 10;
            int heltTime = 0;
            while (true) {
                await Task.Delay(TimeSpan.FromMilliseconds(delayMS));
                heltTime += delayMS;
                if (mouseDevice.LeftButton != MouseButtonState.Pressed || (timeOut >= 0 && heltTime >= timeOut)) {
                    return heltTime;
                }
            }
        }
    }
    public static class VisualTreeUtil {
        public static T? FindVisualParent<T>(DependencyObject? dp) where T : DependencyObject {
            while (dp is not null) {
                if (dp is T t) {
                    return t;
                }
                dp = VisualTreeHelper.GetParent(dp);
            }
            return null;
        }
        public static DependencyObject? FindVisualParent(DependencyObject? dp, Type targetType) {
            while (dp is not null) {
                if (dp.GetType() == targetType) {
                    return dp;
                }
                dp = VisualTreeHelper.GetParent(dp);
            }
            return null;
        }

        public static T? FindVisualChild<T>(DependencyObject? obj) where T : DependencyObject {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++) {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (child is T result) {
                    return result;
                }
                else {
                    T? childOfChild = FindVisualChild<T>(child);
                    if (childOfChild is not null) {
                        return childOfChild;
                    }
                }
            }
            return null;
        }
        public static DependencyObject? FindVisualChild(DependencyObject? obj, Type targetType) {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++) {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (child.GetType() == targetType) {
                    return child;
                }
                else {
                    var childOfChild = FindVisualChild(child, targetType);
                    if (childOfChild is not null) {
                        return childOfChild;
                    }
                }
            }
            return null;
        }
    }
}