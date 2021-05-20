using System.Collections;
using System.Collections.Generic;

namespace Juice
{
	public class CollectionBoxer<TExposed, TBoxed> : IReadOnlyObservableCollection<TExposed> where TBoxed : struct, TExposed
	{
		public event CollectionAddEventHandler<TExposed> ItemAdded;
		public event CollectionCountChangeEventHandler CountChanged;
		public event CollectionRemoveEventHandler<TExposed> ItemRemoved;
		public event CollectionMoveEventHandler<TExposed> ItemMoved;
		public event CollectionReplaceEventHandler<TExposed> ItemReplaced;
		public event CollectionResetEventHandler Reset;
		public event CollectionChangeEventHandler Changed;

		public int Count => boxedCollection.Count;
		public TExposed this[int index] => boxedCollection[index];

		private readonly IReadOnlyObservableCollection<TBoxed> boxedCollection;

		public CollectionBoxer(IReadOnlyObservableCollection<TBoxed> boxedCollection)
		{
			this.boxedCollection = boxedCollection;
			boxedCollection.ItemAdded += OnItemAdded;
			boxedCollection.CountChanged += OnCountChanged;
			boxedCollection.ItemRemoved += OnItemRemoved;
			boxedCollection.ItemMoved += OnItemMoved;
			boxedCollection.ItemReplaced += OnItemReplaced;
			boxedCollection.Reset += OnReset;
			boxedCollection.Changed += OnChanged;
		}

		private void OnItemAdded(int index, TBoxed value)
		{
			ItemAdded?.Invoke(index, value);
		}

		private void OnCountChanged(int oldCount, int newCount)
		{
			CountChanged?.Invoke(oldCount, newCount);
		}

		private void OnItemRemoved(int index, TBoxed value)
		{
			ItemRemoved?.Invoke(index, value);
		}

		private void OnItemMoved(int oldIndex, int newIndex, TBoxed value)
		{
			ItemMoved?.Invoke(oldIndex, newIndex, value);
		}

		private void OnItemReplaced(int index, TBoxed oldValue, TBoxed newValue)
		{
			ItemReplaced?.Invoke(index, oldValue, newValue);
		}

		private void OnReset()
		{
			Reset?.Invoke();
		}

		private void OnChanged()
		{
			Changed?.Invoke();
		}

		public IEnumerator<TExposed> GetEnumerator()
		{
			foreach (TBoxed current in boxedCollection)
			{
				yield return current;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
