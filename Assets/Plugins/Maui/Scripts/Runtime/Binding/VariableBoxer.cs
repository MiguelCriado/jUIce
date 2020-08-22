namespace Maui
{
	public class VariableBoxer<T, U> : IReadOnlyObservableVariable<T> where U : struct, T
	{
		public event ObservableVariableEventHandler<T> Changed;

		public bool HasValue => boxedVariable.HasValue;
		public T Value => boxedVariable.Value;

		private readonly IReadOnlyObservableVariable<U> boxedVariable;

		public VariableBoxer(IReadOnlyObservableVariable<U> boxedVariable)
		{
			this.boxedVariable = boxedVariable;
			boxedVariable.Changed += BoxedVariableChangedHandler;
		}

		private void BoxedVariableChangedHandler(U newValue)
		{
			Changed?.Invoke(newValue);
		}
	}
}