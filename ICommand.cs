#if !NETFX_CORE

namespace System.Windows.Input
{
    using System;

    public interface ICommand
    {
        event EventHandler CanExecuteChanged;

        bool CanExecute(object parameter);

        void Execute(object parameter);
    }
}

#endif
