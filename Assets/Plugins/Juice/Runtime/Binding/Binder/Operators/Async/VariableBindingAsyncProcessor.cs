using System.Threading.Tasks;
using UnityEngine;

namespace Juice
{
	public abstract class VariableBindingAsyncProcessor<TFrom, TTo> : IBindingProcessor
	{
		public IViewModel ViewModel { get; }

		protected readonly VariableBinding<TFrom> variableBinding;
		protected readonly ObservableVariable<TTo> processedVariable;

		protected VariableBindingAsyncProcessor(BindingInfo bindingInfo, Component context)
		{
			processedVariable = new ObservableVariable<TTo>();
			ViewModel = new OperatorVariableViewModel<TTo>(processedVariable);
			variableBinding = new VariableBinding<TFrom>(bindingInfo, context);
			variableBinding.Property.Changed += BoundVariableChangedHandler;
		}

		public virtual void Bind()
		{
			variableBinding.Bind();
		}

		public virtual void Unbind()
		{
			variableBinding.Unbind();
		}

		protected abstract Task<TTo> ProcessValueAsync(TFrom value);

		protected virtual async void BoundVariableChangedHandler(TFrom newValue)
		{
			processedVariable.Value = await ProcessValueAsync(newValue);
		}
	}
}