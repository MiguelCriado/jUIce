using System.Collections;
using System.Collections.Generic;

namespace Maui
{
	public class CollectionBoxer<T, U> : IReadOnlyObservableCollection<T> where U : struct, T
	{
		public event CollectionAddEventHandler<T> ItemAdded;
		public event CollectionCountChangeEventHandler CountChanged;
		public event CollectionRemoveEventHandler<T> ItemRemoved;
		public event CollectionMoveEventHandler<T> ItemMoved;
		public event CollectionReplaceEventHandler<T> ItemReplaced;
		public event CollectionResetEventHandler Reset;

		public int Count => boxedCollection.Count;
		public T this[int index] => boxedCollection[index];
		
		private readonly IReadOnlyObservableCollection<U> boxedCollection;

		public CollectionBoxer(IReadOnlyObservableCollection<U> boxedCollection)
		{
			this.boxedCollection = boxedCollection;
			boxedCollection.ItemAdded += BoxedCollectionItemAddedHandler;
			boxedCollection.CountChanged += BoxedCollectionCountChangedHandler;
			boxedCollection.ItemRemoved += BoxedCollectionItemRemovedHandler;
			boxedCollection.ItemMoved += BoxedCollectionItemMovedHandler;
			boxedCollection.ItemReplaced += BoxedCollectionItemReplacedHandler;
			boxedCollection.Reset += BoxedCollectionResetHandler;
		}

		private void BoxedCollectionItemAddedHandler(int index, U value)
		{
			ItemAdded?.Invoke(index, value);
		}
		
		private void BoxedCollectionCountChangedHandler(int oldCount, int newCount)
		{
			CountChanged?.Invoke(oldCount, newCount);
		}
		
		private void BoxedCollectionItemRemovedHandler(int index, U value)
		{
			ItemRemoved?.Invoke(index, value);
		}

		private void BoxedCollectionItemMovedHandler(int oldIndex, int newIndex, U value)
		{
			ItemMoved?.Invoke(oldIndex, newIndex, value);
		}
		
		private void BoxedCollectionItemReplacedHandler(int index, U oldValue, U newValue)
		{
			ItemReplaced?.Invoke(index, oldValue, newValue);
		}

		private void BoxedCollectionResetHandler()
		{
			Reset?.Invoke();
		}

		public IEnumerator<T> GetEnumerator()
		{
			foreach (U current in boxedCollection)
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