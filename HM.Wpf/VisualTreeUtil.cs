using System.Security.Cryptography;
using System.IO;
using System;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;

namespace HM.Wpf
{
    public static class VisualTreeUtil
    {
        public static T? FindVisualParent<T>(DependencyObject? dp)
            where T : DependencyObject
        {
            while (dp is not null)
            {
                if (dp is T t)
                {
                    return t;
                }
                dp = VisualTreeHelper.GetParent(dp);
            }
            return null;
        }
        public static T? FindVisualChild<T>(DependencyObject? obj)
            where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (child is T result)
                {
                    return result;
                }
                else
                {
                    T? childOfChild = FindVisualChild<T>(child);
                    if (childOfChild is not null)
                    {
                        return childOfChild;
                    }
                }
            }
            return null;
        }
    }
}