namespace Juice
{
	public class OperatorVariableViewModel<T> : OperatorViewModel
	{
		public IReadOnlyObservableVariable<T> Value { get; }

		public OperatorVariableViewModel(IReadOnlyObservableVariable<T> value)
		{
			Value = value;
		}
	}
}