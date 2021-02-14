namespace Juice
{
	public class OperatorBinderVariableViewModel<T> : OperatorBinderViewModel
	{
		public IReadOnlyObservableVariable<T> Value { get; }

		public OperatorBinderVariableViewModel(IReadOnlyObservableVariable<T> value)
		{
			Value = value;
		}
	}
}