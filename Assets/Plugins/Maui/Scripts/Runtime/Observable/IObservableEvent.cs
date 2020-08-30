namespace Maui
{
	public delegate void ObservableEventDelegate();
	public delegate void ObservableEventDelegate<in T>(T eventData);
	
	public interface IObservableEvent
	{
		event ObservableEventDelegate Raised;
	}

	public interface IObservableEvent<T>
	{
		event ObservableEventDelegate<T> Raised;
	}
}