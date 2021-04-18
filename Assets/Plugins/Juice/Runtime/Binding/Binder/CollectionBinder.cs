using UnityEngine;

namespace Juice
{
	public abstract class CollectionBinder<T> : ComponentBinder
	{
		[SerializeField, Rename(nameof(BindingInfoName))]
		private BindingInfo bindingInfo = new BindingInfo(typeof(IReadOnlyObservableCollection<T>));

		protected virtual string BindingInfoName { get; } = nameof(bindingInfo);

		protected override void Awake()
		{
			base.Awake();

			RegisterCollection<T>(bindingInfo)
				.OnReset(OnCollectionReset)
				.OnCountChanged(OnCollectionCountChanged)
				.OnItemAdded(OnCollectionItemAdded)
				.OnItemMoved(OnCollectionItemMoved)
				.OnItemRemoved(OnCollectionItemRemoved)
				.OnItemReplaced(OnCollectionItemReplaced);
		}

		protected abstract void OnCollectionReset();
		protected abstract void OnCollectionCountChanged(int oldCount, int newCount);
		protected abstract void OnCollectionItemAdded(int index, T value);
		protected abstract void OnCollectionItemMoved(int oldIndex, int newIndex, T value);
		protected abstract void OnCollectionItemRemoved(int index, T value);
		protected abstract void OnCollectionItemReplaced(int index, T oldValue, T newValue);
	}
}
