using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Aurora_UI
{
    public class SubscribeCommand : ICommand
    {
        private readonly MainWindowViewModel _vm;
        public SubscribeCommand(MainWindowViewModel vm)
        {
            _vm = vm;

        }
        public void Execute(object parameter)
        {
            _vm.Subscribed();

        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
