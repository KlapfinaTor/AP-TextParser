using System;
using System.Windows.Input;

namespace AP_TextParser_Klapf
{
    internal class DelegateCommand : ICommand
    {
        private readonly Action<object> execute;
        private readonly Predicate<object> canExecute;

        public DelegateCommand(Predicate<object> canExecute, Action<object> execute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public DelegateCommand(Action<object> execute) : this(null, execute)
        {
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged() => this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public bool CanExecute(object parameter) => this.canExecute?.Invoke(parameter) ?? true;

        public void Execute(object parameter) => this.execute?.Invoke(parameter);
    }
}