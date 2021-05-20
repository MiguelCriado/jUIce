using System;
using UnityEngine;

namespace Juice
{
	public class IntComparerOperator : Operator
	{
		[SerializeField] private BindingInfo operandA = new BindingInfo(typeof(IReadOnlyObservableVariable<int>));
		[SerializeField] private MathComparisonType operation;
		[SerializeField] private IntConstantBindingInfo operandB = new IntConstantBindingInfo();

		private int A => operandABinding.Property.GetValue(0);
		private int B => operandBBinding.Property.GetValue(0);

		private ObservableVariable<bool> result;
		private OperatorVariableViewModel<bool> viewModel;
		private VariableBinding<int> operandABinding;
		private VariableBinding<int> operandBBinding;

		protected override void Awake()
		{
			base.Awake();

			result = new ObservableVariable<bool>();
			viewModel = new OperatorVariableViewModel<bool>(result);
			ViewModel = viewModel;

			operandABinding = RegisterVariable<int>(operandA).OnChanged(OnOperandChanged).GetBinding();
			operandBBinding = RegisterVariable<int>(operandB).OnChanged(OnOperandChanged).GetBinding();
		}

		protected override Type GetInjectionType()
		{
			return typeof(OperatorVariableViewModel<bool>);
		}

		private void OnOperandChanged(int newValue)
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
