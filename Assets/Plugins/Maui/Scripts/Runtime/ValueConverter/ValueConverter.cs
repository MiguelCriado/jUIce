using System;
using UnityEngine;

namespace Maui
{
	public abstract class ValueConverter<TFrom, TTo> : ViewModelComponent, IViewModelInjector
	{
		public Type InjectionType => GetInjectionType();
		public ViewModelComponent Target => this;
		
		[SerializeField] private BindingInfo bindingInfo = new BindingInfo(typeof(IReadOnlyObservableVariable<TFrom>));

		private VariableBinding<TFrom> binding;
		private ObservableVariable<TTo> convertedValue;

		protected void Reset()
		{
			bindingInfo = new BindingInfo(typeof(IReadOnlyObservableVariable<TFrom>));
			expectedType = new SerializableType(typeof(ValueConverterViewModel<TTo>));
		}

		protected virtual void Awake()
		{
			convertedValue = new ObservableVariable<TTo>();
			ViewModel = new ValueConverterViewModel<TTo>(convertedValue);
			binding = new VariableBinding<TFrom>(bindingInfo, this);
			binding.Property.Changed += BoundPropertyChangedHandler;
		}

		protected virtual void OnEnable()
		{
			binding.Bind();
		}

		protected virtual void OnDisable()
		{
			binding.Unbind();
		}

		protected abstract TTo Convert(TFrom value);
		
		private void BoundPropertyChangedHandler(TFrom newValue)
		{
			convertedValue.Value = Convert(newValue);
		}

		private Type GetInjectionType()
		{
			return typeof(ValueConverterViewModel<TTo>);
		}
	}
}