using System.Collections.Generic;

namespace Maui
{
	public delegate void CollectionAddEventHandler<T>(int index, T value);

	public delegate void CollectionCountChangeEventHandler(int oldCount, int newCount);

	public delegate void CollectionRemoveEventHandler<T>(int index, T value);

	public delegate void CollectionMoveEventHandler<T>(int oldIndex, int newIndex, T value);

	public delegate void CollectionReplaceEventHandler<T>(int index, T oldValue, T newValue);

	public delegate void CollectionResetEventHandler();

	public interface IReadOnlyObservableCollection<T> : IEnumerable<T>
	{
		event CollectionAddEventHandler<T> ItemAdded;
		event CollectionCountChangeEventHandler CountChanged;
		event CollectionRemoveEventHandler<T> ItemRemoved;
		event CollectionMoveEventHandler<T> ItemMoved;
		event CollectionReplaceEventHandler<T> ItemReplaced;
		event CollectionResetEventHandler Reset;

		int Count { get; }
		T this[int index] { get; }
	}
}
