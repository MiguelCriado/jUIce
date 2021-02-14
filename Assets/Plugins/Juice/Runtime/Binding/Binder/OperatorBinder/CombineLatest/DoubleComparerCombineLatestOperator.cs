using System;
using UnityEngine;

namespace Juice
{
	public class DoubleComparerCombineLatestOperator : ViewModelComponent, IViewModelInjector
	{
		public Type InjectionType => typeof(OperatorBinderVariableViewModel<bool>);
		public ViewModelComponent Target => this;

		[SerializeField] private BindingInfo operandA = new BindingInfo(typeof(IReadOnlyObservableVariable<double>));
		[SerializeField] private MathComparisonType operation;
		[SerializeField] private DoubleConstantBindingInfo operandB = new DoubleConstantBindingInfo();

		private double A => operandABinding.Property.GetValue(0);
		private double B => operandBBinding.Property.GetValue(0);

		private ObservableVariable<bool> result;
		private OperatorBinderVariableViewModel<bool> viewModel;
		private VariableBinding<double> operandABinding;
		private VariableBinding<double> operandBBinding;

		protected virtual void Awake()
		{
			result = new ObservableVariable<bool>();
			viewModel = new OperatorBinderVariableViewModel<bool>(result);
			ViewModel = viewModel;
			
			operandABinding = new VariableBinding<double>(operandA, this);
			operandABinding.Property.Changed += OperandChangedHandler;
			operandBBinding = new VariableBinding<double>(operandB, this);
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
		
		private void OperandChangedHandler(double newValue)
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