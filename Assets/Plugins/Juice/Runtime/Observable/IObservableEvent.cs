namespace Juice
{
	public delegate void ObservableEventHandler();
	public delegate void ObservableEventHandler<in T>(T eventData);
	
	public interface IObservableEvent
	{
		event ObservableEventHandler Raised;
	}

	public interface IObservableEvent<T>
	{
		event ObservableEventHandler<T> Raised;
	}
}