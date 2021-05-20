using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Juice.Pooling;

namespace Juice
{
	public class ObservableCollection<T> : Collection<T>, IObservableCollection<T>
	{
		public event CollectionAddEventHandler<T> ItemAdded;
		public event CollectionCountChangeEventHandler CountChanged;
		public event CollectionRemoveEventHandler<T> ItemRemoved;
		public event CollectionMoveEventHandler<T> ItemMoved;
		public event CollectionReplaceEventHandler<T> ItemReplaced;
		public event CollectionResetEventHandler Reset;
		public event CollectionChangeEventHandler Changed;

		private readonly EqualityComparer<T> comparer = EqualityComparer<T>.Default;

		private bool collectionChanged = false;

		public ObservableCollection()
		{

		}

		public ObservableCollection(EqualityComparer<T> comparer)
		{
			this.comparer = comparer;
		}

		public ObservableCollection(IEnumerable<T> initialItems)
		{
			AddRange(initialItems);
		}

		public ObservableCollection(EqualityComparer<T> comparer, IEnumerable<T> initialItems)
		{
			this.comparer = comparer;
			AddRange(initialItems);
		}

		public void AddRange(IEnumerable<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}

			collectionChanged = false;

			foreach (T item in collection)
			{
				AddItem(item, false);
				collectionChanged = true;
			}

			if (collectionChanged)
			{
				OnChanged();
			}
		}

		public void Move(int oldIndex, int newIndex)
		{
			MoveItem(oldIndex, newIndex, true);
		}

		public void Set(IEnumerable<T> newContent)
		{
			using (var helperList = PooledList<T>.Get())
			{
				helperList.AddRange(newContent);
				collectionChanged = false;
				Set(0, helperList);

				if (collectionChanged)
				{
					OnChanged();
				}
			}
		}

		protected virtual void Set(int index, List<T> helperList)
		{
			if (index >= Count)
			{
				// new tail
				for (int i = index; i < helperList.Count; i++)
				{
					AddItem(helperList[i], false);
					collectionChanged = true;
				}
			}
			else if (index >= helperList.Count)
			{
				// tail removed
				for (int i = Count - 1; i >= index; i--)
				{
					RemoveItem(i, false);
					collectionChanged = true;
				}
			}
			else if (comparer.Equals(this[index], helperList[index]))
			{
				// item did not change
				Set(index + 1, helperList);
			}
			else
			{
				collectionChanged = true;
				int oldIndex = FindIndex(index + 1, helperList[index]);

				if (oldIndex > -1)
				{
					// newItem moved
					MoveItem(oldIndex, index, false);
					Set(index + 1, helperList);
				}
				else
				{
					// newItem added
					InsertItem(index, helperList[index], false);
					Set(index + 1, helperList);
				}
			}
		}

		protected sealed override void ClearItems()
		{
			ClearItems(true);
		}

		protected virtual void ClearItems(bool raiseChangeEvent)
		{
			int previousCount = Count;
			base.ClearItems();

			OnReset();

			if (previousCount > 0)
			{
				OnCountChanged(previousCount);

				if (raiseChangeEvent)
				{
					OnChanged();
				}
			}
		}

		protected virtual void AddItem(T item, bool raiseChangeEvent)
		{
			InsertItem(Items.Count, item, raiseChangeEvent);
		}

		protected override void InsertItem(int index, T item)
		{
			InsertItem(index, item, true);
		}

		protected virtual void InsertItem(int index, T item, bool raiseChangeEvent)
		{
			base.InsertItem(index, item);

			OnItemAdded(index, item);
			OnCountChanged(Count - 1);

			if (raiseChangeEvent)
			{
				OnChanged();
			}
		}

		protected virtual void MoveItem(int oldIndex, int newIndex, bool raiseChangeEvent)
		{
			T item = this[oldIndex];
			base.RemoveItem(oldIndex);
			base.InsertItem(newIndex, item);

			OnItemMoved(oldIndex, newIndex, item);

			if (raiseChangeEvent)
			{
				OnChanged();
			}
		}

		protected sealed override void RemoveItem(int index)
		{
			RemoveItem(index, true);
		}

		protected virtual void RemoveItem(int index, bool raiseChangeEvent)
		{
			T item = this[index];
			base.RemoveItem(index);

			OnItemRemoved(index, item);
			OnCountChanged(Count + 1);

			if (raiseChangeEvent)
			{
				OnChanged();
			}
		}

		protected sealed override void SetItem(int index, T item)
		{
			SetItem(index, item, true);
		}

		protected virtual void SetItem(int index, T item, bool raiseChangeEvent)
		{
			T oldItem = this[index];
			base.SetItem(index, item);

			OnItemReplaced(index, oldItem, item);

			if (raiseChangeEvent)
			{
				OnChanged();
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

		protected virtual void OnItemAdded(int index, T item)
		{
			ItemAdded?.Invoke(index, item);
		}

		protected virtual void OnItemMoved(int oldIndex, int newIndex, T item)
		{
			ItemMoved?.Invoke(oldIndex, newIndex, item);
		}

		protected virtual void OnItemRemoved(int index, T item)
		{
			ItemRemoved?.Invoke(index, item);
		}

		protected virtual void OnItemReplaced(int index, T oldValue, T newValue)
		{
			ItemReplaced?.Invoke(index, oldValue, newValue);
		}

		protected virtual void OnChanged()
		{
			Changed?.Invoke();
		}

		private int FindIndex(int startIndex, T item)
		{
			int result = -1;
			int i = startIndex;

			while (result < 0 && i < Count)
			{
				if (comparer.Equals(this[i], item))
				{
					result = i;
				}

				i++;
			}

			return result;
		}
	}
}
