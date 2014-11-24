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
        /// <summary>
        /// Construct a NotifyCollectionChangedEventArgs that describes a reset change.
        /// </summary>
        /// <param name="action">The action that caused the event (must be Reset).</param>
        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action)
        {
            this.action = action;
            if (action != NotifyCollectionChangedAction.Reset)
                throw new ArgumentException("This constructor can only be used with the Reset action.", "action");
        }
        
        /// <summary>
        /// Construct a NotifyCollectionChangedEventArgs that describes a one-item change.
        /// </summary>
        /// <param name="action">The action that caused the event; can only be Reset, Add or Remove action.</param>
        /// <param name="changedItem">The item affected by the change.</param>
		public NotifyCollectionChangedEventArgs (NotifyCollectionChangedAction action, object changedItem)
			: this (action, changedItem, -1)
		{
		}

		/// <summary>
        /// Construct a NotifyCollectionChangedEventArgs that describes a one-item change.
        /// </summary>
        /// <param name="action">The action that caused the event.</param>
        /// <param name="changedItem">The item affected by the change.</param>
        /// <param name="index">The index where the change occurred.</param>
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
        
        /// <summary>
        /// Construct a NotifyCollectionChangedEventArgs that describes a multi-item change.
        /// </summary>
        /// <param name="action">The action that caused the event.</param>
        /// <param name="changedItems">The items affected by the change.</param>
        public NotifyCollectionChangedEventArgs (NotifyCollectionChangedAction action, IList changedItems)
			: this (action, changedItems, -1)
		{
		}
        
        /// <summary>
        /// Construct a NotifyCollectionChangedEventArgs that describes a multi-item change (or a reset).
        /// </summary>
        /// <param name="action">The action that caused the event.</param>
        /// <param name="changedItems">The items affected by the change.</param>
        /// <param name="startingIndex">The index where the change occurred.</param>
        public NotifyCollectionChangedEventArgs (NotifyCollectionChangedAction action, IList changedItems, int startingIndex)
			: this (action, changedItems, startingIndex)
		{
		}
        
        /// <summary>
        /// Construct a NotifyCollectionChangedEventArgs that describes a one-item Replace event.
        /// </summary>
        /// <param name="action">Can only be a Replace action.</param>
        /// <param name="newItem">The new item replacing the original item.</param>
        /// <param name="oldItem">The original item that is replaced.</param>        
       	public NotifyCollectionChangedEventArgs (NotifyCollectionChangedAction action, object newItem, object oldItem)
			: this (action, newItem, oldItem, -1)
		{
		}        

        /// <summary>
        /// Construct a NotifyCollectionChangedEventArgs that describes a one-item Replace event.
        /// </summary>
        /// <param name="action">Can only be a Replace action.</param>
        /// <param name="newItem">The new item replacing the original item.</param>
        /// <param name="oldItem">The original item that is replaced.</param>
        /// <param name="index">The index of the item being replaced.</param>
        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem, int index)
        {
            this.action = action;
            if (action != NotifyCollectionChangedAction.Replace)
                throw new ArgumentException("This constructor can only be used with the Replace action.", "action");
            InitializeReplace(new object[] { newItem }, new object[] { oldItem }, index);
        }
        
        /// <summary>
        /// Construct a NotifyCollectionChangedEventArgs that describes a multi-item Replace event.
        /// </summary>
        /// <param name="action">Can only be a Replace action.</param>
        /// <param name="newItems">The new items replacing the original items.</param>
        /// <param name="oldItems">The original items that are replaced.</param>
		public NotifyCollectionChangedEventArgs (NotifyCollectionChangedAction action, IList newItems, IList oldItems)
			: this (action, newItems, oldItems, -1)
		{
		}
		
        /// <summary>
        /// Construct a NotifyCollectionChangedEventArgs that describes a multi-item Replace event.
        /// </summary>
        /// <param name="action">Can only be a Replace action.</param>
        /// <param name="newItems">The new items replacing the original items.</param>
        /// <param name="oldItems">The original items that are replaced.</param>
        /// <param name="startingIndex">The starting index of the items being replaced.</param>		
		public NotifyCollectionChangedEventArgs (NotifyCollectionChangedAction action, IList newItems, IList oldItems, int startingIndex)
			: this (action, newItems, oldItems, startingIndex)
		{
		}

        /// <summary>
        /// Construct a NotifyCollectionChangedEventArgs that describes a one-item Move event.
        /// </summary>
        /// <param name="action">Can only be a Move action.</param>
        /// <param name="changedItem">The item affected by the change.</param>
        /// <param name="index">The new index for the changed item.</param>
        /// <param name="oldIndex">The old index for the changed item.</param>
		public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem, int index, int oldIndex)
            : this(action, new object[] { changedItem }, index, oldIndex)
        {
        }

        /// <summary>
        /// Construct a NotifyCollectionChangedEventArgs that describes a multi-item Move event.
        /// </summary>
        /// <param name="action">The action that caused the event.</param>
        /// <param name="changedItems">The items affected by the change.</param>
        /// <param name="index">The new index for the changed items.</param>
        /// <param name="oldIndex">The old index for the changed items.</param>		
        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems, int index, int oldIndex)
        {
            this.action = action;
            if (action != NotifyCollectionChangedAction.Move)
                throw new ArgumentException("This constructor can only be used with the Move action.", "action");
            if (index < -1)
                throw new ArgumentException("The value of index must be -1 or greater.", "index");
            InitializeMove(changedItems, index, oldIndex);
        }

        /// <summary>
        /// Construct a NotifyCollectionChangedEventArgs with given fields (no validation). Used by WinRT marshaling.
        /// </summary>
        internal NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems, int newIndex, int oldIndex)
        {
            this.action = action;            
            this.newItems = (newItems == null) ? null : ArrayList.ReadOnly(newItems);            
            this.oldItems = (oldItems == null) ? null : ArrayList.ReadOnly(oldItems);
            this.newIndex = newIndex;
            this.oldIndex = oldIndex;
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
}

#endif
