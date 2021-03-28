using System;
using UnityEngine;

namespace Juice
{
	public abstract class StartWithOperator<T> : Operator
	{
		protected abstract ConstantBindingInfo<T> InitialValue { get; }
			
		[SerializeField] private BindingInfo source = new BindingInfo(typeof(IReadOnlyObservableVariable<T>));

		private VariableBinding<T> sourceBinding;
		private VariableBinding<T> initialBinding;
		private ObservableVariable<T> exposedProperty;

		private void Awake()
		{
			sourceBinding = new VariableBinding<T>(source, this);
			initialBinding = new VariableBinding<T>(InitialValue, this);
			exposedProperty = new ObservableVariable<T>();
			ViewModel = new OperatorVariableViewModel<T>(exposedProperty);
			
			sourceBinding.Property.Changed += OnBoundPropertyChanged;
			sourceBinding.Property.Cleared += OnBoundPropertyCleared;
		}

		private void OnEnable()
		{
			initialBinding.Bind();

			if (initialBinding.Property.HasValue)
			{
				exposedProperty.Value = initialBinding.Property.Value;
			}
			
			sourceBinding.Bind();
		}

		private void OnDisable()
		{
			sourceBinding.Unbind();
			initialBinding.Unbind();
		}

		protected override Type GetInjectionType()
		{
			return typeof(OperatorVariableViewModel<T>);
		}

		private void OnBoundPropertyChanged(T newValue)
		{
			exposedProperty.Value = newValue;
		}

		private void OnBoundPropertyCleared()
		{
			exposedProperty.Clear();
		}
	}
}