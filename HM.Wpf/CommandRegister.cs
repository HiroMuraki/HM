using System.Windows.Input;
using System;

namespace HM.Wpf
{
    public static class CommandBindingCollectionExtensions
    {
        public static void Register(this CommandBindingCollection self, ICommand command, ExecutedRoutedEventHandler executed, CanExecuteRoutedEventHandler canExecute)
        {
            var cb = new CommandBinding();
            cb.Command = command;
            cb.Executed += executed;
            cb.CanExecute += canExecute;
            self.Add(cb);
        }
        public static void Register(this CommandBindingCollection self, ICommand command, ExecutedRoutedEventHandler executed)
        {
            Register(self, command, executed, AlwaysCanExecute);
        }

        private static void AlwaysCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}