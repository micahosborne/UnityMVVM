#if !NETFX_CORE

namespace System.Collections.Specialized
{
    /// <summary>Describes the action that caused a CollectionChanged event. </summary>
    public enum NotifyCollectionChangedAction
    {
        /// <summary>One or more items were added to the collection.</summary>
        Add,
        /// <summary>One or more items were removed from the collection.</summary>
        Remove,
        /// <summary>One or more items were replaced in the collection.</summary>
        Replace,
        /// <summary>One or more items were moved within the collection.</summary>
        Move,
        /// <summary>The content of the collection changed dramatically.</summary>
        Reset
    }

	/// <summary>Provides data for the CollectionChanged event.</summary>
    public class NotifyCollectionChangedEventArgs : EventArgs
    {
        #region Properties
        /// <summary>Gets the action that caused the event. </summary>
        /// <returns>A NotifyCollectionChangedAction value that describes the action that caused the event.</returns>
        public NotifyCollectionChangedAction Action
        {
            get { return action; }
        }

        /// <summary>Gets the new item involved in the change.</summary>
        /// <returns>The new item involved in the change.</returns>
        public IList NewItems
        {
            get { return newItems; }
        }

        /// <summary>Gets the item affected by a Replace, Remove, or Move action.</summary>
        /// <returns>The item affected by a Replace, Remove, or Move action.</returns>
        public IList OldItems
        {
            get { return oldItems; }
        }

        /// <summary>Gets the index at which the change occurred.</summary>
        /// <returns>The zero-based index at which the change occurred.</returns>
        public int NewStartingIndex
        {
            get { return newIndex; }
        }

        /// <summary>Gets the index at which a Move, Remove, or Replace action occurred.</summary>
        /// <returns>The zero-based index at which a Move Remove, or Replace action occurred.</returns>
        public int OldStartingIndex
        {
            get { return oldIndex; }
        }
        #endregion    
    
        #region Constructors
        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action)
        {
            this.action = action;
            if (action != NotifyCollectionChangedAction.Reset)
                throw new ArgumentException("This constructor can only be used with the Reset action.", "action");
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem, int index)
        {
            IList changedItems = new object[] { changedItem };
            this.action = action;
            if (action == NotifyCollectionChangedAction.Add)
                InitializeAdd(changedItems, index);
            else if (action == NotifyCollectionChangedAction.Remove)
                InitializeRemove(changedItems, index);
            else if (action == NotifyCollectionChangedAction.Reset)
            {
                if (changedItem != null)
                    throw new ArgumentException("This constructor can only be used with the Reset action if changedItem is null", "changedItem");
                if (index != -1)
                    throw new ArgumentException("This constructor can only be used with the Reset action if index is -1", "index");
            }
            else
            {
                throw new ArgumentException("This constructor can only be used with the Reset, Add, or Remove actions.", "action");
            }
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems, int index, int oldIndex)
        {
            this.action = action;
            if (action != NotifyCollectionChangedAction.Move)
                throw new ArgumentException("This constructor can only be used with the Move action.", "action");
            if (index < -1)
                throw new ArgumentException("The value of index must be -1 or greater.", "index");
            InitializeMove(changedItems, index, oldIndex);
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem, int index, int oldIndex)
            : this(action, new object[] { changedItem }, index, oldIndex)
        {
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem, int index)
        {
            this.action = action;
            if (action != NotifyCollectionChangedAction.Replace)
                throw new ArgumentException("This constructor can only be used with the Replace action.", "action");
            InitializeReplace(new object[] { newItem }, new object[] { oldItem }, index);
        }
        #endregion

        #region Initialize Methods
        private void InitializeAdd(IList items, int index)
        {
            this.newItems = ArrayList.ReadOnly(items);
            this.newIndex = index;
        }

        private void InitializeRemove(IList items, int index)
        {
            this.oldItems = ArrayList.ReadOnly(items);
            this.oldIndex = index;
        }

        private void InitializeMove(IList changedItems, int newItemIndex, int oldItemIndex)
        {
            InitializeAdd(changedItems, newItemIndex);
            InitializeRemove(changedItems, oldItemIndex);
        }

        private void InitializeReplace(IList addedItems, IList removedItems, int index)
        {
            InitializeAdd(addedItems, index);
            InitializeRemove(removedItems, index);
        }
        #endregion

        #region Private members
        private NotifyCollectionChangedAction action;
        private IList oldItems, newItems;
        private int oldIndex = -1, newIndex = -1;
        #endregion
    }

    /// <summary>Represents the method that handles the INotifyCollectionChanged.CollectionChanged event. </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">Information about the event.</param>
    public delegate void NotifyCollectionChangedEventHandler(object sender, NotifyCollectionChangedEventArgs e);

    /// <summary>Notifies listeners of dynamic changes, such as when items get added and removed or the whole list is refreshed. </summary>
    interface INotifyCollectionChanged
    {
        event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}

#endif
