using System;
using System.Windows.Input;

namespace VSRESTClient.UI.Commands
{
    public class ParameterizedCommand : ICommand
    {
        private readonly Action<object> m_Action;
        private readonly Func<object, bool> _canExecute;

        public ParameterizedCommand(Action<object> action, Func<object, bool> canExecute = null)
        {
            m_Action = action;
            _canExecute = canExecute == null ? (x => true) : canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
                m_Action(parameter);
        }
    }
}
