using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Aurora_UI.Commands
{
    public class UnsubscribeCommand:ICommand
    {
        #region ICommand Members

        private MainWindowViewModel _vm;
        public  UnsubscribeCommand(MainWindowViewModel vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _vm.Unsubscribed();
        }

        #endregion
    }
}
