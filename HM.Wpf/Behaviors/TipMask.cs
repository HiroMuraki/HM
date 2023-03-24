using System.Windows;

namespace HM.Wpf.Behaviors;

public sealed class TipMask : Mask
{
    public override double InitOpacity => 0;

    protected override void OnAttached()
    {
        base.OnAttached();

        Hide();
        AssociatedObject.IsEnabled = false;
    }

    protected override void Show()
    {
        AssociatedObject.Visibility = Visibility.Visible;

        var animation = CreateDoubleAnimation(InitOpacity, MaskedOpacity);
        AssociatedObject.BeginAnimation(UIElement.OpacityProperty, animation);
    }
    protected override void Hide()
    {
        AssociatedObject.Visibility = Visibility.Collapsed;
    }
}