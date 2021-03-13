namespace Juice
{
	public class VariableBoxer<TExposed, TBoxed> : IReadOnlyObservableVariable<TExposed> where TBoxed : struct, TExposed
	{
		public event ObservableVariableEventHandler<TExposed> Changed;
		public event ObservableVariableClearEventHandler Cleared;

		public bool HasValue => boxedVariable.HasValue;
		public TExposed Value => boxedVariable.Value;

		private readonly IReadOnlyObservableVariable<TBoxed> boxedVariable;

		public VariableBoxer(IReadOnlyObservableVariable<TBoxed> boxedVariable)
		{
			this.boxedVariable = boxedVariable;
			boxedVariable.Changed += OnBoxedVariableChanged;
			boxedVariable.Cleared -= OnBoxedVariableCleared;
		}

		public TExposed GetValue(TExposed fallback)
		{
			return boxedVariable.GetValue((TBoxed)fallback);
		}

		private void OnBoxedVariableChanged(TBoxed newValue)
		{
			Changed?.Invoke(newValue);
		}

		private void OnBoxedVariableCleared()
		{
			Cleared?.Invoke();
		}
	}
}
