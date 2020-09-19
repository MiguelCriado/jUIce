namespace Maui
{
	public class ObservableEvent : IObservableEvent
	{
		public event ObservableEventDelegate Raised;

		public void Raise()
		{
			OnRaised();
		}

		protected virtual void OnRaised()
		{
			Raised?.Invoke();
		}
	}

	public class ObservableEvent<T> : IObservableEvent<T>
	{
		public event ObservableEventDelegate<T> Raised;

		public void Raise(T value)
		{
			OnRaised(value);
		}

		protected virtual void OnRaised(T value)
		{
			Raised?.Invoke(value);
		}
	}
}