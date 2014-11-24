#if !NETFX_CORE

namespace System.Collections.Specialized
{   
    /// <summary>Notifies listeners of dynamic changes, such as when items get added and removed or the whole list is refreshed. </summary>
    public interface INotifyCollectionChanged
    {
        event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}

#endif
