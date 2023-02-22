using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HM.Wpf.Behaviors;

public class TextChangedBehavior : Behavior<TextBox>
{
    public static readonly DependencyProperty TriggerIntervalProperty = DependencyProperty.Register(
        nameof(TriggerInterval),
        typeof(int),
        typeof(TextChangedBehavior),
        new PropertyMetadata(0)
    );

    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
        nameof(Command), 
        typeof(ICommand), 
        typeof(TextChangedBehavior), 
        new PropertyMetadata(default)
    );

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public int TriggerInterval
    {
        get => (int)GetValue(TriggerIntervalProperty);
        set => SetValue(TriggerIntervalProperty, value);
    }

    protected override void OnAttached()
    {
        AssociatedObject.TextChanged += AssociatedObject_TextChanged;
    }

    private Timer? _timer;
    private void AssociatedObject_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (Command is null) return;

        _timer?.Dispose();

        _timer = new Timer(_ =>
        {
            _timer?.Dispose();
            _timer = null;
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (Command.CanExecute(null))
                {
                    Command.Execute(AssociatedObject.Text);
                }
            });
        }, null, TriggerInterval, Timeout.Infinite);
    }
}
