namespace Juice
{
	public class EventBindingSubscriber
	{
		private readonly EventBinding binding;

		public EventBindingSubscriber(EventBinding binding)
		{
			this.binding = binding;
		}

		public EventBindingSubscriber OnRaised(ObservableEventHandler callback)
		{
			binding.Property.Raised += callback;
			return this;
		}

		public EventBinding GetBinding()
		{
			return binding;
		}
	}

	public class EventBindingSubscriber<T>
	{
		private readonly EventBinding<T> binding;

		public EventBindingSubscriber(EventBinding<T> binding)
		{
			this.binding = binding;
		}

		public EventBindingSubscriber<T> OnRaised(ObservableEventHandler<T> callback)
		{
			binding.Property.Raised += callback;
			return this;
		}

		public EventBinding<T> GetBinding()
		{
			return binding;
		}
	}
}
