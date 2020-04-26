using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Muui
{
	public class ObservableCollection<T> : Collection<T>, IObservableCollection<T>
	{
		public event CollectionAddDelegate<T> ItemAdded;
		public event CollectionCountChangeDelegate CountChanged;
		public event CollectionRemoveDelegate<T> ItemRemoved;
		public event CollectionMoveEvent<T> ItemMoved;
		public event CollectionReplaceEvent<T> ItemReplaced;
		public event CollectionResetEvent Reset;

		public ObservableCollection()
		{

		}

		public ObservableCollection(IEnumerable<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}

			foreach (T item in collection)
			{
				Add(item);
			}
		}

		public ObservableCollection(List<T> list) : base(list != null ? new List<T>(list) : null)
		{

		}

		public void Move(int oldIndex, int newIndex)
		{
			MoveItem(oldIndex, newIndex);
		}

		protected override void ClearItems()
		{
			int previousCount = Count;
			base.ClearItems();

			OnReset();

			if (previousCount > 0)
			{
				OnCountChanged(previousCount);
			}
		}

		protected virtual void OnReset()
		{
			Reset?.Invoke();
		}

		protected virtual void OnCountChanged(int previousCount)
		{
			CountChanged?.Invoke(previousCount, Count);
		}

		protected override void InsertItem(int index, T item)
		{
			base.InsertItem(index, item);

			OnItemAdded(index, item);
			OnCountChanged(Count - 1);
		}

		protected virtual void OnItemAdded(int index, T item)
		{
			ItemAdded?.Invoke(index, item);
		}

		protected virtual void MoveItem(int oldIndex, int newIndex)
		{
			T item = this[oldIndex];
			base.RemoveItem(oldIndex);
			base.InsertItem(newIndex, item);

			OnItemMoved(oldIndex, newIndex, item);
		}

		protected virtual void OnItemMoved(int oldIndex, int newIndex, T item)
		{
			ItemMoved?.Invoke(oldIndex, newIndex, item);
		}

		protected override void RemoveItem(int index)
		{
			T item = this[index];
			base.RemoveItem(index);

			OnItemRemoved(index, item);
			OnCountChanged(Count + 1);
		}

		protected virtual void OnItemRemoved(int index, T item)
		{
			ItemRemoved?.Invoke(index, item);
		}

		protected override void SetItem(int index, T item)
		{
			T oldItem = this[index];
			base.SetItem(index, item);

			OnItemReplaced(index, oldItem, item);
		}

		protected virtual void OnItemReplaced(int index, T oldValue, T newValue)
		{
			ItemReplaced?.Invoke(index, oldValue, newValue);
		}
	}
}
