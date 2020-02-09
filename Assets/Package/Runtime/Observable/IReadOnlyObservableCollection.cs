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
		event CollectionAddDelegate<T> OnAdd;
		event CollectionCountChangeDelegate OnCountChange;
		event CollectionRemoveDelegate<T> OnRemove;
		event CollectionMoveEvent<T> OnMove;
		event CollectionReplaceEvent<T> OnReplace;
		event CollectionResetEvent OnReset;

		int Count { get; }
		T this[int index] { get; }
	}
}
