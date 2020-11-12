using System;
using UnityEngine;

namespace Maui
{
	public class LongComparerCombineLatestOperator : ViewModelComponent, IViewModelInjector
	{
		public Type InjectionType => typeof(OperatorBinderVariableViewModel<bool>);
		public ViewModelComponent Target => this;

		[SerializeField] private BindingInfo operandA = new BindingInfo(typeof(IReadOnlyObservableVariable<long>));
		[SerializeField] private MathComparisonType operation;
		[SerializeField] private LongConstantBindingInfo operandB = new LongConstantBindingInfo();

		private long A => operandABinding.Property.GetValue(0);
		private long B => operandBBinding.Property.GetValue(0);

		private ObservableVariable<bool> result;
		private OperatorBinderVariableViewModel<bool> viewModel;
		private VariableBinding<long> operandABinding;
		private VariableBinding<long> operandBBinding;

		protected virtual void Awake()
		{
			result = new ObservableVariable<bool>();
			viewModel = new OperatorBinderVariableViewModel<bool>(result);
			ViewModel = viewModel;
			
			operandABinding = new VariableBinding<long>(operandA, this);
			operandABinding.Property.Changed += OperandChangedHandler;
			operandBBinding = new VariableBinding<long>(operandB, this);
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
		
		private void OperandChangedHandler(long newValue)
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