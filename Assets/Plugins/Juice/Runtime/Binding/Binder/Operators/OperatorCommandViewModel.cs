namespace Juice
{
	public class OperatorCommandViewModel<T> : OperatorViewModel
	{
		public IObservableCommand<T> Value { get; }

		public OperatorCommandViewModel(IObservableCommand<T> value)
		{
			Value = value;
		}
	}
}