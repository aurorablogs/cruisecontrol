using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Aurora_UI
{
    class CancelOrderCommand:ICommand
    {
        private readonly MainWindowViewModel _vm;
        public CancelOrderCommand(MainWindowViewModel vm)
        {
            _vm = vm;
        }

        public void Execute(object parameter)
        {
            _vm.CancelOrderExecute();

        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
