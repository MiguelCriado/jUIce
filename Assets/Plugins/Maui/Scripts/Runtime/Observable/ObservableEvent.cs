namespace Maui
{
	public class ObservableEvent : IObservableEvent
	{
		public event ObservableEventDelegate Raised;

		public void Raise()
		{
			OnTriggered();
		}

		protected virtual void OnTriggered()
		{
			Raised?.Invoke();
		}
	}

	public class ObservableEvent<T> : IObservableEvent<T>
	{
		public event ObservableEventDelegate<T> Raised;

		public void Raise(T value)
		{
			OnTriggered(value);
		}

		protected virtual void OnTriggered(T value)
		{
			Raised?.Invoke(value);
		}
	}
}