using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GUI.ViewsModels
{
    internal class ViewModelCommand : ICommand
    {
        //Fields
        private readonly Action<object> _executeAction;
        private readonly Predicate<object>? _canExecuteAction;
        //Constructors
        public ViewModelCommand(Action<object> executeAction)
        {
            _executeAction = executeAction;
            _canExecuteAction = null;
        }
        public ViewModelCommand(Action<object> executeAction, Predicate<object> canExecuteAction)
        {
            _executeAction = executeAction;
            _canExecuteAction = canExecuteAction;
        }
        //Events
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        //Methods
#pragma warning disable CS8767 // Допустимость значений NULL для ссылочных типов в типе параметра не соответствует неявно реализованному элементу (возможно, из-за атрибутов допустимости значений NULL).
        public bool CanExecute(object parameter)
#pragma warning restore CS8767 // Допустимость значений NULL для ссылочных типов в типе параметра не соответствует неявно реализованному элементу (возможно, из-за атрибутов допустимости значений NULL).
        {
            return _canExecuteAction == null ? true : _canExecuteAction(parameter);
        }
#pragma warning disable CS8767 // Допустимость значений NULL для ссылочных типов в типе параметра не соответствует неявно реализованному элементу (возможно, из-за атрибутов допустимости значений NULL).
        public void Execute(object parameter) => _executeAction(parameter);
#pragma warning restore CS8767 // Допустимость значений NULL для ссылочных типов в типе параметра не соответствует неявно реализованному элементу (возможно, из-за атрибутов допустимости значений NULL).
    }
}
