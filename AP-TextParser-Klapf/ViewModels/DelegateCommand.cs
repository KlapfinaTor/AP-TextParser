using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AP_TextParser_Klapf
{
    class DelegateCommand : ICommand
    {
        readonly Action<object> execute;
        readonly Predicate<object> canExecute;

        public DelegateCommand(Predicate<object> canExecute, Action<object> execute )
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }
        public DelegateCommand(Action<object> execute) : this(null,execute ) { }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged() =>this.CanExecuteChanged?.Invoke(this,EventArgs.Empty);

        public bool CanExecute(object parameter) => this.canExecute?.Invoke(parameter) ?? true;

        public void Execute(object parameter) => this.execute?.Invoke(parameter);
    }
}
