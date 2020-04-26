using System.Collections.Generic;

namespace Muui
{
	public delegate void CollectionAddDelegate<T>(int index, T value);

	public delegate void CollectionCountChangeDelegate(int oldCount, int newCount);

	public delegate void CollectionRemoveDelegate<T>(int index, T value);

	public delegate void CollectionMoveEvent<T>(int oldIndex, int newIndex, T value);

	public delegate void CollectionReplaceEvent<T>(int index, T oldValue, T newValue);

	public delegate void CollectionResetEvent();

	public interface IReadOnlyObservableCollection<T> : IEnumerable<T>
	{
		event CollectionAddDelegate<T> ItemAdded;
		event CollectionCountChangeDelegate CountChanged;
		event CollectionRemoveDelegate<T> ItemRemoved;
		event CollectionMoveEvent<T> ItemMoved;
		event CollectionReplaceEvent<T> ItemReplaced;
		event CollectionResetEvent Reset;

		int Count { get; }
		T this[int index] { get; }
	}
}
