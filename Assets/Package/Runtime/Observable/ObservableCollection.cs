using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Muui
{
	public class ObservableCollection<T> : Collection<T>, IObservableCollection<T>
	{
		public event CollectionAddDelegate<T> OnAdd;
		public event CollectionCountChangeDelegate OnCountChange;
		public event CollectionRemoveDelegate<T> OnRemove;
		public event CollectionMoveEvent<T> OnMove;
		public event CollectionReplaceEvent<T> OnReplace;
		public event CollectionResetEvent OnReset;

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

			OnReset?.Invoke();

			if (previousCount > 0)
			{
				OnCountChange?.Invoke(previousCount, Count);
			}
		}

		protected override void InsertItem(int index, T item)
		{
			base.InsertItem(index, item);

			OnAdd?.Invoke(index, item);
			OnCountChange?.Invoke(Count - 1, Count);
		}

		protected virtual void MoveItem(int oldIndex, int newIndex)
		{
			T item = this[oldIndex];
			base.RemoveItem(oldIndex);
			base.InsertItem(newIndex, item);

			OnMove?.Invoke(oldIndex, newIndex, item);
		}

		protected override void RemoveItem(int index)
		{
			T item = this[index];
			base.RemoveItem(index);

			OnRemove?.Invoke(index, item);
			OnCountChange?.Invoke(Count + 1, Count);
		}

		protected override void SetItem(int index, T item)
		{
			T oldItem = this[index];
			base.SetItem(index, item);

			OnReplace?.Invoke(index, oldItem, item);
		}
	}
}
