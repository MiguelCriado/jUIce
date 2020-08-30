namespace Maui
{
	public class OperatorBinderCommandViewModel<T> : IViewModel
	{
		public IObservableCommand<T> Value { get; }

		public OperatorBinderCommandViewModel(IObservableCommand<T> value)
		{
			Value = value;
		}
	}
}