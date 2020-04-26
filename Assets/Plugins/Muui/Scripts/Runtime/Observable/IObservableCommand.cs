namespace Muui
{
	public delegate void ObservableCommandDelegate<T>(T parameter);
	public delegate void ObservableCommandDelegate();

	public interface IObservableCommand
	{
		event ObservableCommandDelegate ExecuteRequested;

		IReadOnlyObservableVariable<bool> CanExecute { get; }

		bool Execute();
	}

	public interface IObservableCommand<T>
	{
		event ObservableCommandDelegate<T> ExecuteRequested;

		IReadOnlyObservableVariable<bool> CanExecute { get; }

		bool Execute(T parameter);
	}
}
