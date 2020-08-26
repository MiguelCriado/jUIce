namespace Maui
{
	public class OperatorBinderVariableViewModel<T> : IViewModel
	{
		public IReadOnlyObservableVariable<T> Value { get; }

		public OperatorBinderVariableViewModel(IReadOnlyObservableVariable<T> value)
		{
			Value = value;
		}
	}
}