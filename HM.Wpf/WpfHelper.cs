using System.Windows;

namespace HM.Wpf
{
    public static class WpfHelper
    {
        public static T? GetObjectFromTag<T>(object? obj)
            where T : class
        {
            return (obj as FrameworkElement)?.Tag as T;
        }
    }
}