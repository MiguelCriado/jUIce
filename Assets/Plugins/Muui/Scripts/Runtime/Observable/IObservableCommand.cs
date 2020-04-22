namespace Muui
{
	public delegate void ObservableCommandDelegate<T>(T parameter);
	public delegate void ObservableCommandDelegate();

	public interface IObservableCommand
	{
		event ObservableCommandDelegate OnRequestExecute;

		IReadOnlyObservableVariable<bool> CanExecute { get; }

		bool Execute();
	}

	public interface IObservableCommand<T>
	{
		event ObservableCommandDelegate<T> OnRequestExecute;

		IReadOnlyObservableVariable<bool> CanExecute { get; }

		bool Execute(T parameter);
	}
}
