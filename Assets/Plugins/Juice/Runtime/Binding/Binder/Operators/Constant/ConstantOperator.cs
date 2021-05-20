using System;

namespace Juice
{
	public abstract class ConstantOperator<T> : Operator
	{
		protected abstract ConstantBindingInfo Value { get; }

		private ObservableVariable<T> exposedVariable;

		protected override void Awake()
		{
			base.Awake();

			exposedVariable = new ObservableVariable<T>();
			ViewModel = new OperatorVariableViewModel<T>(exposedVariable);

			RegisterVariable<T>(Value)
				.OnChanged(OnChanged)
				.OnCleared(OnCleared);
		}

		protected override Type GetInjectionType()
		{
			return typeof(OperatorVariableViewModel<T>);
		}

		private void OnChanged(T newValue)
		{
			exposedVariable.Value = newValue;
		}

		private void OnCleared()
		{
			exposedVariable.Clear();
		}
	}
}
