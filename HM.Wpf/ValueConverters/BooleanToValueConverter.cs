using System.Globalization;
using System.Windows.Data;

namespace HM.Wpf.ValueConverters;

[ValueConversion(typeof(bool), typeof(Enum))]
public sealed class BooleanToValueConverter : IValueConverter
{
    public static BooleanToValueConverter Default { get; } = new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value.Equals(parameter);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool isChecked = (bool)value;

        if (!isChecked)
        {
            return Binding.DoNothing;
        }

        return parameter;
    }
}