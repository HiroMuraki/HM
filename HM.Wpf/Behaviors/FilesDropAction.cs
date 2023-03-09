using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Input;

namespace HM.Wpf.Behaviors;

public sealed class FilesDropAction : Behavior<FrameworkElement>
{
    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
        nameof(Command),
        typeof(ICommand),
        typeof(FilesDropAction),
        new PropertyMetadata(null)
    );
    public static readonly DependencyProperty IsActivitingProperty = DependencyProperty.Register(
        nameof(IsActiviting),
        typeof(bool),
        typeof(FilesDropAction),
        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
    );

    public bool IsActiviting
    {
        get => (bool)GetValue(IsActivitingProperty);
        set => SetValue(IsActivitingProperty, value);
    }

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        AssociatedObject.DragEnter += (_, e) =>
        {
            IsActiviting = true;
        };
        AssociatedObject.DragLeave += (_, e) =>
        {
            IsActiviting = false;
        };
        AssociatedObject.Drop += (_, e) =>
        {
            IsActiviting = false;

            if (Command is null) return;

            var files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (files is null || files.Length <= 0)
            {
                return;
            }

            if (Command.CanExecute(files))
            {
                Command.Execute(files);
            }
        };
    }
}