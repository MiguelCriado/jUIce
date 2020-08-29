using System;
using UnityEngine;

namespace Maui
{
	public enum MathComparisonType
	{
		Equals,
		NotEquals,
		Greater,
		GreaterOrEquals,
		Less,
		LessOrEquals
	}
	
	public class FloatComparerCombineLatestOperator : ViewModelComponent, IViewModelInjector
	{
		public Type InjectionType => typeof(OperatorBinderVariableViewModel<float>);
		public ViewModelComponent Target => this;

		[SerializeField] private BindingInfo operandA = new BindingInfo(typeof(IReadOnlyObservableVariable<float>));
		[SerializeField] private MathComparisonType operation;
		[SerializeField] private FloatConstantBindingInfo operandB = new FloatConstantBindingInfo();
		
	}
}