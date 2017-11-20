using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BlackListConsole.Command
{
    public abstract class BlackListCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public BlackListCommand()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(null, null);
        }

        public abstract void Execute(object parameter);       
    }
}
