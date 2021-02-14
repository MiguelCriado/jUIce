namespace Juice
{
	public class OperatorBinderEventViewModel<T> : OperatorBinderViewModel
	{
		public IObservableEvent<T> Value { get; }

		public OperatorBinderEventViewModel(IObservableEvent<T> value)
		{
			Value = value;
		}
	}
}