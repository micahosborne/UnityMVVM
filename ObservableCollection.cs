using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime;
using System.Runtime.CompilerServices;

namespace System.Collections.ObjectModel
{
  /// <summary>
  /// Represents a dynamic data collection that provides notifications when items get added, removed, or when the whole list is refreshed.
  /// </summary>
  /// <typeparam name="T">The type of elements in the collection.</typeparam>
  [Serializable]
  public class ObservableCollection<T> : Collection<T>, INotifyCollectionChanged, INotifyPropertyChanged
  {
    private ObservableCollection<T>.SimpleMonitor _monitor = new ObservableCollection<T>.SimpleMonitor();
    private const string CountString = "Count";
    private const string IndexerName = "Item[]";

    event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
    {
     add
      {
        this.PropertyChanged += value;
      }
     remove
      {
        this.PropertyChanged -= value;
      }
    }

    /// <summary>
    /// Occurs when an item is added, removed, changed, moved, or the entire list is refreshed.
    /// </summary>
    public virtual event NotifyCollectionChangedEventHandler CollectionChanged;

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    protected virtual event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Collections.ObjectModel.ObservableCollection`1"/> class.
    /// </summary>
    public ObservableCollection()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Collections.ObjectModel.ObservableCollection`1"/> class that contains elements copied from the specified list.
    /// </summary>
    /// <param name="list">The list from which the elements are copied.</param><exception cref="T:System.ArgumentNullException">The <paramref name="list"/> parameter cannot be null.</exception>
    public ObservableCollection(List<T> list)
      : base(list != null ? (IList<T>) new List<T>(list.Count) : (IList<T>) list)
    {
      this.CopyFrom((IEnumerable<T>) list);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Collections.ObjectModel.ObservableCollection`1"/> class that contains elements copied from the specified collection.
    /// </summary>
    /// <param name="collection">The collection from which the elements are copied.</param><exception cref="T:System.ArgumentNullException">The <paramref name="collection"/> parameter cannot be null.</exception>
    public ObservableCollection(IEnumerable<T> collection)
    {
      if (collection == null)
        throw new ArgumentNullException("collection");
      this.CopyFrom(collection);
    }

    private void CopyFrom(IEnumerable<T> collection)
    {
      IList<T> items = this.Items;
      if (collection == null || items == null)
        return;
      foreach (T obj in collection)
        items.Add(obj);
    }

    /// <summary>
    /// Moves the item at the specified index to a new location in the collection.
    /// </summary>
    /// <param name="oldIndex">The zero-based index specifying the location of the item to be moved.</param><param name="newIndex">The zero-based index specifying the new location of the item.</param>
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public void Move(int oldIndex, int newIndex)
    {
      this.MoveItem(oldIndex, newIndex);
    }

    /// <summary>
    /// Removes all items from the collection.
    /// </summary>
    protected override void ClearItems()
    {
      this.CheckReentrancy();
      base.ClearItems();
      this.OnPropertyChanged("Count");
      this.OnPropertyChanged("Item[]");
      this.OnCollectionReset();
    }

    /// <summary>
    /// Removes the item at the specified index of the collection.
    /// </summary>
    /// <param name="index">The zero-based index of the element to remove.</param>
    protected override void RemoveItem(int index)
    {
      this.CheckReentrancy();
      T obj = this[index];
      base.RemoveItem(index);
      this.OnPropertyChanged("Count");
      this.OnPropertyChanged("Item[]");
      this.OnCollectionChanged(NotifyCollectionChangedAction.Remove, (object) obj, index);
    }

    /// <summary>
    /// Inserts an item into the collection at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param><param name="item">The object to insert.</param>
    protected override void InsertItem(int index, T item)
    {
      this.CheckReentrancy();
      base.InsertItem(index, item);
      this.OnPropertyChanged("Count");
      this.OnPropertyChanged("Item[]");
      this.OnCollectionChanged(NotifyCollectionChangedAction.Add, (object) item, index);
    }

    /// <summary>
    /// Replaces the element at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the element to replace.</param><param name="item">The new value for the element at the specified index.</param>
    protected override void SetItem(int index, T item)
    {
      this.CheckReentrancy();
      T obj = this[index];
      base.SetItem(index, item);
      this.OnPropertyChanged("Item[]");
      this.OnCollectionChanged(NotifyCollectionChangedAction.Replace, (object) obj, (object) item, index);
    }

    /// <summary>
    /// Moves the item at the specified index to a new location in the collection.
    /// </summary>
    /// <param name="oldIndex">The zero-based index specifying the location of the item to be moved.</param><param name="newIndex">The zero-based index specifying the new location of the item.</param>
    protected virtual void MoveItem(int oldIndex, int newIndex)
    {
      this.CheckReentrancy();
      T obj = this[oldIndex];
      base.RemoveItem(oldIndex);
      base.InsertItem(newIndex, obj);
      this.OnPropertyChanged("Item[]");
      this.OnCollectionChanged(NotifyCollectionChangedAction.Move, (object) obj, newIndex, oldIndex);
    }

    /// <summary>
    /// Raises the <see cref="E:System.Collections.ObjectModel.ObservableCollection`1.PropertyChanged"/> event with the provided arguments.
    /// </summary>
    /// <param name="e">Arguments of the event being raised.</param>
    protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, e);
    }

    /// <summary>
    /// Raises the <see cref="E:System.Collections.ObjectModel.ObservableCollection`1.CollectionChanged"/> event with the provided arguments.
    /// </summary>
    /// <param name="e">Arguments of the event being raised.</param>
    protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
      if (this.CollectionChanged == null)
        return;
      using (this.BlockReentrancy())
        this.CollectionChanged((object) this, e);
    }

    /// <summary>
    /// Disallows reentrant attempts to change this collection.
    /// </summary>
    /// 
    /// <returns>
    /// An <see cref="T:System.IDisposable"/> object that can be used to dispose of the object.
    /// </returns>
    protected IDisposable BlockReentrancy()
    {
      this._monitor.Enter();
      return (IDisposable) this._monitor;
    }

    /// <summary>
    /// Checks for reentrant attempts to change this collection.
    /// </summary>
    /// <exception cref="T:System.InvalidOperationException">If there was a call to <see cref="M:System.Collections.ObjectModel.ObservableCollection`1.BlockReentrancy"/> of which the <see cref="T:System.IDisposable"/> return value has not yet been disposed of. Typically, this means when there are additional attempts to change this collection during a <see cref="E:System.Collections.ObjectModel.ObservableCollection`1.CollectionChanged"/> event. However, it depends on when derived classes choose to call <see cref="M:System.Collections.ObjectModel.ObservableCollection`1.BlockReentrancy"/>.</exception>
    protected void CheckReentrancy()
    {
      if (this._monitor.Busy && this.CollectionChanged != null && this.CollectionChanged.GetInvocationList().Length > 1)
        throw new InvalidOperationException(SR.GetString("ObservableCollectionReentrancyNotAllowed"));
    }

    private void OnPropertyChanged(string propertyName)
    {
      this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
    }

    private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index)
    {
      this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));
    }

    private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index, int oldIndex)
    {
      this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index, oldIndex));
    }

    private void OnCollectionChanged(NotifyCollectionChangedAction action, object oldItem, object newItem, int index)
    {
      this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index));
    }

    private void OnCollectionReset()
    {
      this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    [TypeForwardedFrom("WindowsBase, Version=3.0.0.0, Culture=Neutral, PublicKeyToken=31bf3856ad364e35")]
    [Serializable]
    private class SimpleMonitor : IDisposable
    {
      private int _busyCount;

      public bool Busy
      {
        get
        {
          return this._busyCount > 0;
        }
      }

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      public SimpleMonitor()
      {
      }

      public void Enter()
      {
        ++this._busyCount;
      }

      public void Dispose()
      {
        --this._busyCount;
      }
    }
  }
}
