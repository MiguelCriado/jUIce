namespace Juice
{
	public class OperatorBinderCommandViewModel<T> : OperatorBinderViewModel
	{
		public IObservableCommand<T> Value { get; }

		public OperatorBinderCommandViewModel(IObservableCommand<T> value)
		{
			Value = value;
		}
	}
}