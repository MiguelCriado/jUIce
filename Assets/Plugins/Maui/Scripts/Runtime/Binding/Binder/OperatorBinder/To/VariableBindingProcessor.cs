using System;
using UnityEngine;

namespace Maui
{
	public class VariableBindingProcessor<TFrom, TTo> : BindingProcessor<TFrom, TTo>
	{
		public override IViewModel ViewModel { get; }

		private readonly VariableBinding<TFrom> variableBinding;
		private readonly ObservableVariable<TTo> processedVariable;

		public VariableBindingProcessor(BindingInfo bindingInfo, Component context, Func<TFrom, TTo> processFunction)
			: base(bindingInfo, context, processFunction)
		{
			processedVariable = new ObservableVariable<TTo>();
			ViewModel = new OperatorBinderVariableViewModel<TTo>(processedVariable);
			variableBinding = new VariableBinding<TFrom>(bindingInfo, context);
			variableBinding.Property.Changed += BoundVariableChangedHandler;
		}

		public override void Bind()
		{
			variableBinding.Bind();
		}

		public override void Unbind()
		{
			variableBinding.Unbind();
		}
		
		private void BoundVariableChangedHandler(TFrom newValue)
		{
			processedVariable.Value = processFunction(newValue);
		}
	}
}