namespace Juice
{
	public class OperatorEventViewModel<T> : OperatorViewModel
	{
		public IObservableEvent<T> Value { get; }

		public OperatorEventViewModel(IObservableEvent<T> value)
		{
			Value = value;
		}
	}
}