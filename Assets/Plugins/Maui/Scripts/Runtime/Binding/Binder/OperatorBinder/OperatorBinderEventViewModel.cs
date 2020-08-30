namespace Maui
{
	public class OperatorBinderEventViewModel<T> : IViewModel
	{
		public IObservableEvent<T> Value { get; }

		public OperatorBinderEventViewModel(IObservableEvent<T> value)
		{
			Value = value;
		}
	}
}