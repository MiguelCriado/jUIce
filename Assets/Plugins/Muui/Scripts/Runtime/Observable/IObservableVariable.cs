namespace Maui
{
	public interface IObservableVariable<T> : IReadOnlyObservableVariable<T>
	{
		new T Value { get; set; }
	}
}

