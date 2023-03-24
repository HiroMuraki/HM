using System.Windows;
using System.Windows.Controls;

namespace HM.Wpf.Behaviors;

public sealed class Dockable : AnimatedBehavior<FrameworkElement>
{
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
        nameof(Orientation),
        typeof(Orientation),
        typeof(Dockable),
        new FrameworkPropertyMetadata(Orientation.Horizontal)
    );
    public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(
        nameof(IsExpanded),
        typeof(bool),
        typeof(Dockable),
        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IsExpandedValueChanged)
    );
    public static readonly DependencyProperty ExpandedSizeProperty = DependencyProperty.Register(
        nameof(ExpandedSize),
        typeof(double),
        typeof(Dockable),
        new FrameworkPropertyMetadata(0.0)
    );
    public static readonly DependencyProperty FoldedSizeProperty = DependencyProperty.Register(
        nameof(FoldedSize),
        typeof(double),
        typeof(Dockable),
        new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
    );

    public bool IsExpanded
    {
        get => (bool)GetValue(IsExpandedProperty);
        set => SetValue(IsExpandedProperty,
            value);
    }
    public double FoldedSize
    {
        get => (double)GetValue(FoldedSizeProperty);
        set => SetValue(FoldedSizeProperty, value);
    }
    public double ExpandedSize
    {
        get => (double)GetValue(ExpandedSizeProperty);
        set => SetValue(ExpandedSizeProperty, value);
    }
    public Orientation Orientation
    {
        get => (Orientation)GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        if (Orientation == Orientation.Horizontal)
        {
            AssociatedObject.Width = FoldedSize;
        }
        else if (Orientation == Orientation.Vertical)
        {
            AssociatedObject.Height = FoldedSize;
        }
    }

    private void Expand()
    {
        var animation = CreateDoubleAnimation(FoldedSize, ExpandedSize);

        if (Orientation == Orientation.Horizontal)
        {
            AssociatedObject.BeginAnimation(FrameworkElement.WidthProperty, animation);
        }
        else if (Orientation == Orientation.Vertical)
        {
            AssociatedObject.BeginAnimation(FrameworkElement.HeightProperty, animation);
        }
    }
    private void Fold()
    {
        var animation = CreateDoubleAnimation(ExpandedSize, FoldedSize);

        if (Orientation == Orientation.Horizontal)
        {
            AssociatedObject.BeginAnimation(FrameworkElement.WidthProperty, animation);
        }
        else if (Orientation == Orientation.Vertical)
        {
            AssociatedObject.BeginAnimation(FrameworkElement.HeightProperty, animation);
        }
    }
    private static void IsExpandedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is null) return;

        var dockable = (Dockable)d;

        if ((bool)e.NewValue)
        {
            dockable.Expand();
        }
        else
        {
            dockable.Fold();
        }
    }
}