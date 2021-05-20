using System.Collections.Generic;

namespace Juice
{
	public delegate void CollectionAddEventHandler<in T>(int index, T value);
	public delegate void CollectionCountChangeEventHandler(int oldCount, int newCount);
	public delegate void CollectionRemoveEventHandler<in T>(int index, T value);
	public delegate void CollectionMoveEventHandler<in T>(int oldIndex, int newIndex, T value);
	public delegate void CollectionReplaceEventHandler<in T>(int index, T oldValue, T newValue);
	public delegate void CollectionResetEventHandler();
	public delegate void CollectionChangeEventHandler();

	public interface IReadOnlyObservableCollection<out T> : IEnumerable<T>
	{
		event CollectionAddEventHandler<T> ItemAdded;
		event CollectionCountChangeEventHandler CountChanged;
		event CollectionRemoveEventHandler<T> ItemRemoved;
		event CollectionMoveEventHandler<T> ItemMoved;
		event CollectionReplaceEventHandler<T> ItemReplaced;
		event CollectionResetEventHandler Reset;
		event CollectionChangeEventHandler Changed;

		int Count { get; }
		T this[int index] { get; }
	}
}
