using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Aurora_UI.Commands
{
    class SelectionChangedCommand:ICommand
    {
        private readonly MainWindowViewModel _vm;
        public SelectionChangedCommand(MainWindowViewModel vm)
        {
            _vm = vm;
        }

        public void Execute(object parameter)
        {
           _vm.SelectionChanged();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
