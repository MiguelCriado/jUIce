namespace Muui
{
	public delegate void ObservableVariableEventHandler<T>(T newValue);

	public interface IReadOnlyObservableVariable<T>
	{
		event ObservableVariableEventHandler<T> Changed;

		bool HasValue { get; }
		T Value { get; }
	}
}
