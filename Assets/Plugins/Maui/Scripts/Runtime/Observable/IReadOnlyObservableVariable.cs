namespace Maui
{
	public delegate void ObservableVariableEventHandler<in T>(T newValue);
	public delegate void ObservableVariableClearEventHandler();

	public interface IReadOnlyObservableVariable<out T>
	{
		event ObservableVariableEventHandler<T> Changed;
		event ObservableVariableClearEventHandler Cleared;

		bool HasValue { get; }
		T Value { get; }
	}
}
