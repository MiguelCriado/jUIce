namespace Muui
{
	public delegate void ObservableVariableDelegate<T>(T newValue);

	public interface IReadOnlyObservableVariable<T>
	{
		event ObservableVariableDelegate<T> Changed;

		bool HasValue { get; }
		T Value { get; }
	}
}
