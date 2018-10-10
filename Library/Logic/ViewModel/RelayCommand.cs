using System;
using System.Windows.Input;

namespace Library.Logic.ViewModel
{
    public class RelayCommand : ICommand
    {
        public RelayCommand(Action execute)
            : this(execute, ()=>true)
        { }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException();
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        private Action _execute;
        private Func<bool> _canExecute;
    }
}
