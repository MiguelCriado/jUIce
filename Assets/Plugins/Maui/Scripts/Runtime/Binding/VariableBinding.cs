using System;
using System.Reflection;
using UnityEngine;

namespace Maui
{
	public class VariableBinding<T> : Binding
	{
		public IReadOnlyObservableVariable<T> Property => exposedProperty;

		private readonly ObservableVariable<T> exposedProperty;
		private IReadOnlyObservableVariable<T> boundProperty;
		private ObservableVariableAdapter boundPropertyAdapter;

		public VariableBinding(BindingInfo bindingInfo, Component context) : base(bindingInfo, context)
		{
			exposedProperty = new ObservableVariable<T>();
		}

		protected override Type GetBindingType()
		{
			return typeof(IReadOnlyObservableVariable<T>);
		}

		protected override void BindProperty(object property)
		{
			boundProperty = property as IReadOnlyObservableVariable<T>;

			if (boundProperty == null && BindingUtils.CanBeAdapted(property.GetType(), typeof(IReadOnlyObservableVariable<T>)))
			{
				boundPropertyAdapter = new ObservableVariableAdapter(property);
			}

			if (boundProperty != null)
			{
				boundProperty.Changed += BoundPropertyChangedHandler;
				BoundPropertyChangedHandler(boundProperty.Value);
			}
			else if (boundPropertyAdapter != null)
			{
				boundPropertyAdapter.Changed += BoundPropertyAdapterChangedHandler;
				BoundPropertyAdapterChangedHandler(boundPropertyAdapter.Value);
			}
			else
			{
				Debug.LogError($"Property type ({property.GetType()}) different from expected type ({typeof(IReadOnlyObservableVariable<T>)})");
			}
		}

		protected override void UnbindProperty()
		{
			if (boundProperty != null)
			{
				boundProperty.Changed -= BoundPropertyChangedHandler;
				boundProperty = null;
			}

			if (boundPropertyAdapter != null)
			{
				boundPropertyAdapter.Changed -= BoundPropertyAdapterChangedHandler;
				boundPropertyAdapter = null;
			}
		}

		private void BoundPropertyChangedHandler(T newValue)
		{
			exposedProperty.Value = newValue;
		}
		
		private void BoundPropertyAdapterChangedHandler(object newValue)
		{
			exposedProperty.Value = (T) newValue;
		}
	}
}