using System;
using System.Collections.Generic;
using Maui.Utils;
using UnityEngine;

namespace Maui
{
	public class VariableBinding<T> : Binding
	{
		public override bool IsBound => boundProperty != null;
		public IReadOnlyObservableVariable<T> Property => exposedProperty;

		private readonly ObservableVariable<T> exposedProperty;
		private IReadOnlyObservableVariable<T> boundProperty;

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

			if (boundProperty == null && BindingUtils.NeedsToBeBoxed(property.GetType(), typeof(IReadOnlyObservableVariable<T>)))
			{
				boundProperty = BoxVariable(property);
			}

			if (boundProperty != null)
			{
				boundProperty.Changed += OnBoundPropertyChanged;
				RaiseFirstNotification();
			}
			else
			{
				Debug.LogError($"Property type ({property.GetType()}) cannot be bound as {typeof(IReadOnlyObservableVariable<T>)}", context);
			}
		}

		protected override void UnbindProperty()
		{
			if (boundProperty != null)
			{
				boundProperty.Changed -= OnBoundPropertyChanged;
				boundProperty = null;
			}
		}

		protected override void BindConstant(BindingInfo info)
		{
			if (info is ConstantBindingInfo<T> constantInfo)
			{
				boundProperty = new ObservableVariable<T>(constantInfo.Constant);
				RaiseFirstNotification();
			}
		}

		private static IReadOnlyObservableVariable<T> BoxVariable(object variableToBox)
		{
			IReadOnlyObservableVariable<T> result = null;
			
			Type variableGenericType = variableToBox.GetType().GetGenericTypeTowardsRoot();

			if (variableGenericType != null)
			{
				Type actualType = typeof(T);
				Type boxedType = variableGenericType.GenericTypeArguments[0];
				Type activationType = typeof(VariableBoxer<,>).MakeGenericType(actualType, boxedType);
				result = Activator.CreateInstance(activationType, variableToBox) as IReadOnlyObservableVariable<T>;
			}

			return result;
		}
		
		private void RaiseFirstNotification()
		{
			if (EqualityComparer<T>.Default.Equals(boundProperty.Value, exposedProperty.Value))
			{
				exposedProperty.ForceChangedNotification();
			}
			else
			{
				OnBoundPropertyChanged(boundProperty.Value);
			}
		}
		
		private void OnBoundPropertyChanged(T newValue)
		{
			exposedProperty.Value = newValue;
		}
	}
}