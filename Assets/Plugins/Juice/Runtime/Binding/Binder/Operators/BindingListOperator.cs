using System;
using System.Collections.Generic;
using UnityEngine;

namespace Juice
{
	public abstract class BindingListOperator<TFrom, TTo> : ViewModelComponent, IViewModelInjector
	{
		public Type InjectionType => typeof(OperatorVariableViewModel<TTo>);

		public ViewModelComponent Target => this;

		[SerializeField] private BindingInfoList fromBinding = new BindingInfoList(typeof(IReadOnlyObservableVariable<TFrom>));

		private BindingList<TFrom> bindingList;
		private ObservableVariable<TTo> exposedProperty;

		protected virtual void Awake()
		{
			exposedProperty = new ObservableVariable<TTo>();
			ViewModel = new OperatorVariableViewModel<TTo>(exposedProperty);

			bindingList = new BindingList<TFrom>(this, fromBinding);
			bindingList.VariableChanged += BindingListVariableChangedHandler;
		}

		protected virtual void OnEnable()
		{
			bindingList.Bind();
		}

		protected virtual void OnDisable()
		{
			bindingList.Unbind();
		}

		protected abstract TTo Process(int triggerIndex, IReadOnlyList<TFrom> list);

		private void BindingListVariableChangedHandler(int index, TFrom newValue)
		{
			exposedProperty.Value = Process(index, bindingList.Values);
		}
	}
}
