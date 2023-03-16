using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace HM.Wpf.Behaviors;

public abstract class Mask : AnimatedBehavior<UIElement>
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

    public virtual double InitOpacity
    {
        get => (double)GetValue(InitOpacityProperty);
        set => SetValue(InitOpacityProperty, value);
    }
    public virtual double MaskedOpacity
    {
        get => (double)GetValue(MaskedOpacityProperty);
        set => SetValue(MaskedOpacityProperty, value);
    }
    public virtual bool IsShown
    {
        get => (bool)GetValue(IsShownProperty);
        set => SetValue(IsShownProperty, value);
    }

    protected abstract void Show();
    protected abstract void Hide();
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
}

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