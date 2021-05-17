namespace Juice
{
	public class VariableBindingSubscriber<T>
	{
		private readonly VariableBinding<T> binding;

		public VariableBindingSubscriber(VariableBinding<T> binding)
		{
			this.binding = binding;
		}

		public VariableBindingSubscriber<T> OnChanged(ObservableVariableEventHandler<T> callback)
		{
			binding.Property.Changed += callback;
			return this;
		}

		public VariableBindingSubscriber<T> OnCleared(ObservableVariableClearEventHandler callback)
		{
			binding.Property.Cleared += callback;
			return this;
		}

		public VariableBinding<T> GetBinding()
		{
			return binding;
		}
	}
}
