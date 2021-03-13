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

		public int Count => boxedCollection.Count;
		public TExposed this[int index] => boxedCollection[index];

		private readonly IReadOnlyObservableCollection<TBoxed> boxedCollection;

		public CollectionBoxer(IReadOnlyObservableCollection<TBoxed> boxedCollection)
		{
			this.boxedCollection = boxedCollection;
			boxedCollection.ItemAdded += BoxedCollectionItemAddedHandler;
			boxedCollection.CountChanged += BoxedCollectionCountChangedHandler;
			boxedCollection.ItemRemoved += BoxedCollectionItemRemovedHandler;
			boxedCollection.ItemMoved += BoxedCollectionItemMovedHandler;
			boxedCollection.ItemReplaced += BoxedCollectionItemReplacedHandler;
			boxedCollection.Reset += BoxedCollectionResetHandler;
		}

		private void BoxedCollectionItemAddedHandler(int index, TBoxed value)
		{
			ItemAdded?.Invoke(index, value);
		}

		private void BoxedCollectionCountChangedHandler(int oldCount, int newCount)
		{
			CountChanged?.Invoke(oldCount, newCount);
		}

		private void BoxedCollectionItemRemovedHandler(int index, TBoxed value)
		{
			ItemRemoved?.Invoke(index, value);
		}

		private void BoxedCollectionItemMovedHandler(int oldIndex, int newIndex, TBoxed value)
		{
			ItemMoved?.Invoke(oldIndex, newIndex, value);
		}

		private void BoxedCollectionItemReplacedHandler(int index, TBoxed oldValue, TBoxed newValue)
		{
			ItemReplaced?.Invoke(index, oldValue, newValue);
		}

		private void BoxedCollectionResetHandler()
		{
			Reset?.Invoke();
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
