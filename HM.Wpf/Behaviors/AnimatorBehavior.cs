using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Media.Animation;

namespace HM.Wpf.Behaviors;

public abstract class AnimatorBehavior<T> : Behavior<T>
    where T : DependencyObject
{
    public static readonly DependencyProperty AnimationTimeProperty = DependencyProperty.Register(
        nameof(AnimationTime),
        typeof(double),
        typeof(AnimatorBehavior<T>),
        new PropertyMetadata(200.0)
    );

    public double AnimationTime
    {
        get => (double)GetValue(AnimationTimeProperty);
        set => SetValue(AnimationTimeProperty, value);
    }

    protected DoubleAnimation CreateDoubleAnimation(double from, double to)
    {
        return new DoubleAnimation()
        {
            From = from,
            To = to,
            Duration = TimeSpan.FromMilliseconds(AnimationTime),
            AccelerationRatio = 0.2,
            DecelerationRatio = 0.8
        };
    }
}
