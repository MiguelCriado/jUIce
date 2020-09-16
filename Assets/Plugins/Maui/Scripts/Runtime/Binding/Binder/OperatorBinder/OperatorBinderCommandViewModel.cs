namespace Maui
{
	public class OperatorBinderCommandViewModel<T> : ViewModel
	{
		public IObservableCommand<T> Value { get; }

		public OperatorBinderCommandViewModel(IObservableCommand<T> value)
		{
			Value = value;
		}
	}
}