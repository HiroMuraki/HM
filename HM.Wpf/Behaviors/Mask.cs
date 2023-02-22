using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace HM.Wpf.Behaviors;

public sealed class Mask : AnimatorBehavior<UIElement>
{
    public static readonly DependencyProperty InitOpacityProperty = DependencyProperty.Register(
        nameof(InitOpacity),
        typeof(double),
        typeof(Mask),
        new FrameworkPropertyMetadata(default)
    );
    public static readonly DependencyProperty MaskedOpacityProperty = DependencyProperty.Register(
        nameof(MaskedOpacity),
        typeof(double),
        typeof(Mask),
        new FrameworkPropertyMetadata(default)
    );
    public static readonly DependencyProperty IsShownProperty = DependencyProperty.Register(
        nameof(IsShown),
        typeof(bool),
        typeof(Mask),
        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IsShownChanged)
    );

    public double InitOpacity
    {
        get => (double)GetValue(InitOpacityProperty);
        set => SetValue(InitOpacityProperty, value);
    }
    public double MaskedOpacity
    {
        get => (double)GetValue(MaskedOpacityProperty);
        set => SetValue(MaskedOpacityProperty, value);
    }
    public bool IsShown
    {
        get => (bool)GetValue(IsShownProperty);
        set => SetValue(IsShownProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        Hide();
    }

    private void Show()
    {
        AssociatedObject.Visibility = Visibility.Visible;
        AssociatedObject.IsEnabled = true;
        AssociatedObject.IsHitTestVisible = true;
        AssociatedObject.MouseLeftButtonDown += HideHandler;
        var animation = CreateDoubleAnimation(InitOpacity, MaskedOpacity);

        AssociatedObject.BeginAnimation(UIElement.OpacityProperty, animation);
    }
    private void Hide()
    {
        AssociatedObject.IsHitTestVisible = false;
        AssociatedObject.MouseLeftButtonDown -= HideHandler;
        AssociatedObject.IsEnabled = false;
        var animation = CreateDoubleAnimation(MaskedOpacity, InitOpacity);

        animation.Completed += (_, _) =>
        {
            AssociatedObject.Visibility = Visibility.Collapsed;
        };

        AssociatedObject.BeginAnimation(UIElement.OpacityProperty, animation);
    }
    private static void IsShownChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is null) return;

        var item = (Mask)d;

        if ((bool)e.NewValue)
        {
            item.Show();
        }
        else
        {
            item.Hide();
        }
    }
    private void HideHandler(object sender, MouseButtonEventArgs e)
    {
        IsShown = false;
        BindingOperations.GetBindingExpression(this, IsShownProperty)?.UpdateSource();
    }
}