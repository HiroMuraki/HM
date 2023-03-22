using System.Windows;

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
