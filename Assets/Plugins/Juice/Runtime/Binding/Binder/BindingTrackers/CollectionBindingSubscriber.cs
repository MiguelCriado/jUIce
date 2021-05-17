namespace Juice
{
	public class CollectionBindingSubscriber<T>
	{
		private readonly CollectionBinding<T> binding;

		public CollectionBindingSubscriber(CollectionBinding<T> binding)
		{
			this.binding = binding;
		}

		public CollectionBindingSubscriber<T> OnItemAdded(CollectionAddEventHandler<T> callback)
		{
			binding.Property.ItemAdded += callback;
			return this;
		}

		public CollectionBindingSubscriber<T> OnCountChanged(CollectionCountChangeEventHandler callback)
		{
			binding.Property.CountChanged += callback;
			return this;
		}

		public CollectionBindingSubscriber<T> OnItemRemoved(CollectionRemoveEventHandler<T> callback)
		{
			binding.Property.ItemRemoved += callback;
			return this;
		}

		public CollectionBindingSubscriber<T> OnItemMoved(CollectionMoveEventHandler<T> callback)
		{
			binding.Property.ItemMoved += callback;
			return this;
		}

		public CollectionBindingSubscriber<T> OnItemReplaced(CollectionReplaceEventHandler<T> callback)
		{
			binding.Property.ItemReplaced += callback;
			return this;
		}

		public CollectionBindingSubscriber<T> OnReset(CollectionResetEventHandler callback)
		{
			binding.Property.Reset += callback;
			return this;
		}

		public CollectionBinding<T> GetBinding()
		{
			return binding;
		}
	}
}
