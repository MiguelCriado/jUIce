using System;
using System.Data;
using UnityEngine;

namespace Maui
{
	public class FloatComparerCombineLatestOperator : ViewModelComponent, IViewModelInjector
	{
		public Type InjectionType => typeof(OperatorBinderVariableViewModel<bool>);
		public ViewModelComponent Target => this;

		[SerializeField] private BindingInfo operandA = new BindingInfo(typeof(IReadOnlyObservableVariable<float>));
		[SerializeField] private MathComparisonType operation;
		[SerializeField] private FloatConstantBindingInfo operandB = new FloatConstantBindingInfo();

		private float A => operandABinding.Property.Value;
		private float B => operandBBinding.Property.Value;

		private ObservableVariable<bool> result;
		private OperatorBinderVariableViewModel<bool> viewModel;
		private VariableBinding<float> operandABinding;
		private VariableBinding<float> operandBBinding;

		protected virtual void Awake()
		{
			result = new ObservableVariable<bool>();
			viewModel = new OperatorBinderVariableViewModel<bool>(result);
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