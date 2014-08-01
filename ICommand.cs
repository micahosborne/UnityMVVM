using System;
using System.ComponentModel;

using System;

namespace System.Windows.Input {
	public interface ICommand {
		bool CanExecute (object parameter);
		void Execute (object parameter);
		event EventHandler CanExecuteChanged;
	}
}
using System.Runtime.CompilerServices;
namespace System.Collections.Specialized {
	public interface INotifyCollectionChanged
	{
		event NotifyCollectionChangedEventHandler CollectionChanged;
	}
}
