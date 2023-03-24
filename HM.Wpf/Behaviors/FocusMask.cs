using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace HM.Wpf.Behaviors;

public sealed class FocusMask : Mask
{
    protected override void OnAttached()
    {
        base.OnAttached();

        AssociatedObject.IsEnabled = false;
        AssociatedObject.IsHitTestVisible = false;
        AssociatedObject.MouseLeftButtonDown -= HideHandler;
        AssociatedObject.Opacity = InitOpacity;
        AssociatedObject.Visibility = Visibility.Collapsed;
    }

    protected override void Show()
    {
        AssociatedObject.IsEnabled = true;
        AssociatedObject.Visibility = Visibility.Visible;
        AssociatedObject.IsHitTestVisible = true;
        AssociatedObject.MouseLeftButtonDown += HideHandler;
        var animation = CreateDoubleAnimation(InitOpacity, MaskedOpacity);

        AssociatedObject.BeginAnimation(UIElement.OpacityProperty, animation);
    }
    protected override void Hide()
    {
        AssociatedObject.IsEnabled = false;
        AssociatedObject.IsHitTestVisible = false;
        AssociatedObject.MouseLeftButtonDown -= HideHandler;
        var animation = CreateDoubleAnimation(MaskedOpacity, InitOpacity);

        animation.Completed += (_, _) =>
        {
            AssociatedObject.Visibility = Visibility.Collapsed;
        };

        AssociatedObject.BeginAnimation(UIElement.OpacityProperty, animation);
    }
    private void HideHandler(object sender, MouseButtonEventArgs e)
    {
        IsShown = false;
        BindingOperations.GetBindingExpression(this, IsShownProperty)?.UpdateSource();
    }
}
