using System.Windows;

namespace HM.Wpf
{
    public static class RoutedEventArgsExtensions
    {
        public static void Handle(this RoutedEventArgs self)
        {
            self.Handled = true;
        }
    }
}