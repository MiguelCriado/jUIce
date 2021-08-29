using System;
using UnityEngine;

namespace Juice
{
	public class EnumComparerOperator<T> : Operator
	{
		[SerializeField] private BindingInfo operandA = BindingInfo.Variable<T>();
		[SerializeField] private ConstantBindingInfo<T> operandB = new ConstantBindingInfo<T>();

		private ObservableVariable<bool> result;
		private VariableBinding<T> operandABinding;
		private VariableBinding<T> operandBBinding;

		protected override void Awake()
		{
			base.Awake();
			
			result = new ObservableVariable<bool>();
			ViewModel = new OperatorVariableViewModel<bool>(result);

			operandABinding = RegisterVariable<T>(operandA).OnChanged(OperandChangedHandler).GetBinding();
			operandBBinding = RegisterVariable<T>(operandB).OnChanged(OperandChangedHandler).GetBinding();
		}

		protected override Type GetInjectionType()
		{
			return typeof(OperatorVariableViewModel<bool>);
		}
		
		private  void OperandChangedHandler(T newValue)
		{
			if (operandABinding.IsBound && operandBBinding.IsBound)
			{
				result.Value = Evaluate();
			}
		}

		private bool Evaluate()
		{
			bool result = operandABinding.Property.Value.Equals(operandBBinding.Property.Value);
			
			return result;
		}
	}
}
