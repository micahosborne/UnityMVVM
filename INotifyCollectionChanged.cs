using System.Runtime.CompilerServices;
namespace System.Collections.Specialized {
	public interface INotifyCollectionChanged
	{
		event NotifyCollectionChangedEventHandler CollectionChanged;
	}
}
