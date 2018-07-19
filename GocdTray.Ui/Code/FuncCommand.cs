using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace GocdTray.Ui.Code
{
    public class FuncCommand<TParameter> : ICommand
    {
        private readonly Predicate<TParameter> canExecute;
        private readonly Action<TParameter> execute;

        public FuncCommand(Action<TParameter> execute, Predicate<TParameter> canExecute=null)
        {
            this.canExecute = canExecute;
            this.execute = execute;
        }

        public bool CanExecute(object parameter) => this.canExecute == null || this.canExecute((TParameter) parameter);

        public void Execute(object parameter) => this.execute((TParameter)parameter);

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}