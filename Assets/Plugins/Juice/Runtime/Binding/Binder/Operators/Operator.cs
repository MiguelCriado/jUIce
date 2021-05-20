﻿using System;

namespace Juice
{
	public abstract class Operator : ViewModelComponent, IViewModelInjector
	{
		public Type InjectionType => GetInjectionType();
		public ViewModelComponent Target => this;

		protected abstract Type GetInjectionType();

		private BindingTracker bindingTracker;

		protected virtual void Awake()
		{
			bindingTracker = new BindingTracker(this);
		}

		protected virtual void OnEnable()
		{
			bindingTracker.Bind();
		}

		protected virtual void OnDisable()
		{
			bindingTracker.Unbind();
		}

		protected VariableBindingSubscriber<T> RegisterVariable<T>(BindingInfo bindingInfo)
		{
			return bindingTracker.RegisterVariable<T>(bindingInfo);
		}

		protected OperatorVariableViewModel<T> ExposeVariable<T>(BindingInfo bindingInfo)
		{
			var variable = new ObservableVariable<T>();
			OperatorVariableViewModel<T> result = new OperatorVariableViewModel<T>(variable);

			RegisterVariable<T>(bindingInfo)
				.OnChanged(value => variable.Value = value)
				.OnCleared(variable.Clear);

			return result;
		}

		protected CollectionBindingSubscriber<T> RegisterCollection<T>(BindingInfo bindingInfo)
		{
			return bindingTracker.RegisterCollection<T>(bindingInfo);
		}

		protected EventBindingSubscriber RegisterEvent(BindingInfo bindingInfo)
		{
			return bindingTracker.RegisterEvent(bindingInfo);
		}

		protected EventBindingSubscriber<T> RegisterEvent<T>(BindingInfo bindingInfo)
		{
			return bindingTracker.RegisterEvent<T>(bindingInfo);
		}

		protected CommandBindingSubscriber RegisterCommand(BindingInfo bindingInfo)
		{
			return bindingTracker.RegisterCommand(bindingInfo);
		}

		protected CommandBindingSubscriber<T> RegisterCommand<T>(BindingInfo bindingInfo)
		{
			return bindingTracker.RegisterCommand<T>(bindingInfo);
		}
	}
}
