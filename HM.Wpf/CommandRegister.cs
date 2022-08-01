using System.Windows.Input;
using System;

namespace HM.Wpf
{
    public class CommandRegister
    {
        public CommandBindingCollection CommandBindings { get; }

        public void Register(ICommand command, ExecutedRoutedEventHandler executed, CanExecuteRoutedEventHandler canExecute)
        {
            var cb = new CommandBinding();
            cb.Command = command;
            cb.Executed += executed;
            cb.CanExecute += canExecute;
            CommandBindings.Add(cb);
        }
        public void Register(ICommand command, ExecutedRoutedEventHandler executed)
        {
            Register(command, executed, AlwaysCanExecute);
        }

        public CommandRegister(CommandBindingCollection commandBindings)
        {
            CommandBindings = commandBindings;
        }

        private void AlwaysCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}