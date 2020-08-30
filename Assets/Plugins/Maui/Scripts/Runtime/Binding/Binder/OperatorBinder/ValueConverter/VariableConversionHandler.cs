using System;
using UnityEngine;

namespace Maui
{
	public class VariableConversionHandler<TFrom, TTo> : ConversionHandler<TFrom, TTo>
	{
		public override IViewModel ViewModel { get; }

		private readonly VariableBinding<TFrom> variableBinding;
		private readonly ObservableVariable<TTo> convertedVariable;

		public VariableConversionHandler(BindingInfo bindingInfo, Component context, Func<TFrom, TTo> conversionFunction)
			: base(bindingInfo, context, conversionFunction)
		{
			convertedVariable = new ObservableVariable<TTo>();
			ViewModel = new OperatorBinderVariableViewModel<TTo>(convertedVariable);
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
			convertedVariable.Value = conversionFunction(newValue);
		}
	}
}