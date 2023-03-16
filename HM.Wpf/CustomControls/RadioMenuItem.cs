using System.Windows;
using System.Windows.Controls;

namespace HM.Wpf;

public class RadioMenuItem : MenuItem
{
    protected override void OnClick()
    {
        base.OnClick();
        SetCurrentValue(IsCheckedProperty, true);
    }

    static RadioMenuItem()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(RadioMenuItem), new FrameworkPropertyMetadata(typeof(RadioMenuItem)));
    }
}
