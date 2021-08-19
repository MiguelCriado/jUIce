using System;
using UnityEngine;

namespace Juice
{
	public class FloatComparerOperator : Operator
	{
		[SerializeField] private BindingInfo operandA = new BindingInfo(typeof(IReadOnlyObservableVariable<float>));
		[SerializeField] private MathComparisonType operation;
		[SerializeField] private ConstantBindingInfo<float> operandB = new ConstantBindingInfo<float>();

		private float A => operandABinding.Property.GetValue(0);
		private float B => operandBBinding.Property.GetValue(0);

		private ObservableVariable<bool> result;
		private OperatorVariableViewModel<bool> viewModel;
		private VariableBinding<float> operandABinding;
		private VariableBinding<float> operandBBinding;

		protected override void Awake()
		{
			base.Awake();

			result = new ObservableVariable<bool>();
			ViewModel = new OperatorVariableViewModel<bool>(result);

			operandABinding = RegisterVariable<float>(operandA).OnChanged(OnOperandChanged).GetBinding();
			operandBBinding = RegisterVariable<float>(operandB).OnChanged(OnOperandChanged).GetBinding();
		}

		protected override Type GetInjectionType()
		{
			return typeof(OperatorVariableViewModel<bool>);
		}

		private void OnOperandChanged(float newValue)
		{
			if (operandABinding.IsBound && operandBBinding.IsBound)
			{
				result.Value = Evaluate();
			}
		}

		private bool Evaluate()
		{
			bool result = false;

			switch (operation)
			{
				case MathComparisonType.Equals: result = A == B; break;
				case MathComparisonType.NotEquals: result = A != B; break;
				case MathComparisonType.Greater: result = A > B; break;
				case MathComparisonType.GreaterOrEquals: result = A >= B; break;
				case MathComparisonType.Less: result = A < B; break;
				case MathComparisonType.LessOrEquals: result = A <= B; break;
			}

			return result;
		}
	}
}
