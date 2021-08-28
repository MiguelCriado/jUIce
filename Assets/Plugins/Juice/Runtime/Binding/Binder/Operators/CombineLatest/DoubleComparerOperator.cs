using System;
using UnityEngine;

namespace Juice
{
	public class DoubleComparerOperator : Operator
	{
		[SerializeField] private BindingInfo operandA = BindingInfo.Variable<double>();
		[SerializeField] private MathComparisonType operation;
		[SerializeField] private ConstantBindingInfo<double> operandB = new ConstantBindingInfo<double>();

		private double A => operandABinding.Property.GetValue(0);
		private double B => operandBBinding.Property.GetValue(0);

		private ObservableVariable<bool> result;
		private VariableBinding<double> operandABinding;
		private VariableBinding<double> operandBBinding;

		protected override void Awake()
		{
			base.Awake();

			result = new ObservableVariable<bool>();
			ViewModel = new OperatorVariableViewModel<bool>(result);

			operandABinding = RegisterVariable<double>(operandA).OnChanged(OnOperandChanged).GetBinding();
			operandBBinding = RegisterVariable<double>(operandB).OnChanged(OnOperandChanged).GetBinding();
		}

		protected override Type GetInjectionType()
		{
			return typeof(OperatorVariableViewModel<bool>);
		}

		private void OnOperandChanged(double newValue)
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
