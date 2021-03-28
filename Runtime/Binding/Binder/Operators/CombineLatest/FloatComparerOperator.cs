using System;
using UnityEngine;

namespace Juice
{
	public class FloatComparerOperator : ViewModelComponent, IViewModelInjector
	{
		public Type InjectionType => typeof(OperatorVariableViewModel<bool>);
		public ViewModelComponent Target => this;

		[SerializeField] private BindingInfo operandA = new BindingInfo(typeof(IReadOnlyObservableVariable<float>));
		[SerializeField] private MathComparisonType operation;
		[SerializeField] private FloatConstantBindingInfo operandB = new FloatConstantBindingInfo();

		private float A => operandABinding.Property.GetValue(0);
		private float B => operandBBinding.Property.GetValue(0);

		private ObservableVariable<bool> result;
		private OperatorVariableViewModel<bool> viewModel;
		private VariableBinding<float> operandABinding;
		private VariableBinding<float> operandBBinding;

		protected virtual void Awake()
		{
			result = new ObservableVariable<bool>();
			viewModel = new OperatorVariableViewModel<bool>(result);
			ViewModel = viewModel;

			operandABinding = new VariableBinding<float>(operandA, this);
			operandABinding.Property.Changed += OperandChangedHandler;
			operandBBinding = new VariableBinding<float>(operandB, this);
			operandBBinding.Property.Changed += OperandChangedHandler;
		}

		protected virtual void OnEnable()
		{
			operandABinding.Bind();
			operandBBinding.Bind();
		}

		protected virtual void OnDisable()
		{
			operandABinding.Unbind();
			operandBBinding.Unbind();
		}

		private void OperandChangedHandler(float newValue)
		{
			result.Value = Evaluate();
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
