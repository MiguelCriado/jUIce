using System;
using UnityEngine;

namespace Juice
{
	public abstract class StartWithOperator<T> : Operator
	{
		protected abstract ConstantBindingInfo<T> InitialValue { get; }

		[SerializeField] private BindingInfo source = new BindingInfo(typeof(IReadOnlyObservableVariable<T>));

		private VariableBinding<T> initialBinding;
		private ObservableVariable<T> exposedProperty;

		protected override void Awake()
		{
			base.Awake();

			RegisterVariable<T>(source)
				.OnChanged(OnBoundPropertyChanged)
				.OnCleared(OnBoundPropertyCleared);

			initialBinding = new VariableBinding<T>(InitialValue, this);
			exposedProperty = new ObservableVariable<T>();
			ViewModel = new OperatorVariableViewModel<T>(exposedProperty);
		}

		protected override void OnEnable()
		{
			initialBinding.Bind();

			if (initialBinding.Property.HasValue)
			{
				exposedProperty.Value = initialBinding.Property.Value;
			}

			base.OnEnable();
		}

		protected override void OnDisable()
		{
			base.OnDisable();

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
