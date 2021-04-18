namespace Juice
{
	public class EventBoxer<TExposed, TBoxed> : IObservableEvent<TExposed> where TBoxed : struct, TExposed
	{
		public event ObservableEventHandler<TExposed> Raised;

		private readonly IObservableEvent<TBoxed> boxedEvent;

		public EventBoxer(IObservableEvent<TBoxed> boxedEvent)
		{
			this.boxedEvent = boxedEvent;
			boxedEvent.Raised += BoxedEventRaisedHandler;
		}

		private void BoxedEventRaisedHandler(TBoxed value)
		{
			Raised?.Invoke(value);
		}
	}
}
