namespace Maui
{
	public delegate void ObservableVariableEventHandler<in T>(T newValue);

	public interface IReadOnlyObservableVariable<out T>
	{
		event ObservableVariableEventHandler<T> Changed;

		bool HasValue { get; }
		T Value { get; }
	}
}
