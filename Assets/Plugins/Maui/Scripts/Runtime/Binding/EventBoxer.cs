namespace Maui
{
	public class EventBoxer<T, U> : IObservableEvent<T> where U : struct, T
	{
		public event ObservableEventDelegate<T> Raised;

		private readonly IObservableEvent<U> boxedEvent;
		
		public EventBoxer(IObservableEvent<U> boxedEvent)
		{
			this.boxedEvent = boxedEvent;
			boxedEvent.Raised += BoxedEventRaisedHandler;
		}

		private void BoxedEventRaisedHandler(U value)
		{
			Raised?.Invoke(value);
		}
	}
}