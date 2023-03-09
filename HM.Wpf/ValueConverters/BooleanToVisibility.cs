using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HM.Wpf.ValueConverters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToVisibility : IValueConverter
    {
        public Visibility BooleanFalseVisiblity { get; set; } = Visibility.Collapsed;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((bool)value)
            {
                case true: return Visibility.Visible;
                case false: return BooleanFalseVisiblity;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Visibility)value switch
            {
                Visibility.Visible => true,
                _ => false,
            };
        }
    }
}
