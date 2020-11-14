namespace Maui
{
	public delegate void ObservableCommandEventHandler<in T>(T parameter);
	public delegate void ObservableCommandEventHandler();

	public interface IObservableCommand
	{
		event ObservableCommandEventHandler ExecuteRequested;

		IObservableVariable<bool> CanExecute { get; }

		bool Execute();
	}

	public interface IObservableCommand<T>
	{
		event ObservableCommandEventHandler<T> ExecuteRequested;

		IObservableVariable<bool> CanExecute { get; }

		bool Execute(T parameter);
	}
}
