using System;
using UnityEngine;

namespace Juice
{
	public class VariableHasValueOperator : Operator
	{
		[SerializeField] private BindingInfo variable = new BindingInfo(typeof(IReadOnlyObservableVariable<object>));

		private VariableBinding<object> variableBinding;
		private OperatorVariableViewModel<bool> viewModel;
		private ObservableVariable<bool> exposedVariable;

		protected override void Awake()
		{
			base.Awake();

			exposedVariable = new ObservableVariable<bool>();
			ViewModel = new OperatorVariableViewModel<bool>(exposedVariable);

			variableBinding = new VariableBinding<object>(variable, this);
			variableBinding.Property.Changed += OnVariableChanged;
			variableBinding.Property.Cleared += OnVariableCleared;
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			variableBinding.Bind();
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			variableBinding.Unbind();

			exposedVariable.Value = false;
		}

		protected override Type GetInjectionType()
		{
			return typeof(OperatorVariableViewModel<bool>);
		}

		private void OnVariableChanged(object newValue)
		{
			Refresh();
		}

		private void OnVariableCleared()
		{
			Refresh();
		}

		private void Refresh()
		{
			exposedVariable.Value = variableBinding.IsBound && variableBinding.Property.HasValue;
		}
	}
}
