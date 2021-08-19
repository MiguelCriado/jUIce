using System;
using Juice.Utils;
using UnityEngine;

namespace Juice
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
				boundProperty = BoxVariable(property, context);
			}

			if (boundProperty != null)
			{
				boundProperty.Changed += OnBoundPropertyChanged;
				boundProperty.Cleared += OnBoundPropertyCleared;
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
				boundProperty.Cleared -= OnBoundPropertyCleared;
				boundProperty = null;
			}

			exposedProperty.Clear();
		}

		protected override void BindConstant(BindingInfo info)
		{
			if (info is ConstantBindingInfo<T> constantInfo)
			{
				boundProperty = exposedProperty;
				exposedProperty.Value = constantInfo.Constant;
			}
		}

		private static IReadOnlyObservableVariable<T> BoxVariable(object variableToBox, Component context)
		{
			IReadOnlyObservableVariable<T> result = null;

			Type variableGenericType = variableToBox.GetType().GetGenericTypeTowardsRoot();

			if (variableGenericType != null)
			{
				try
				{
					Type exposedType = typeof(T);
					Type boxedType = variableGenericType.GenericTypeArguments[0];
					Type activationType = typeof(VariableBoxer<,>).MakeGenericType(exposedType, boxedType);
					result = Activator.CreateInstance(activationType, variableToBox) as IReadOnlyObservableVariable<T>;
				}
#pragma warning disable 618
				catch (ExecutionEngineException)
#pragma warning restore 618
				{
					Debug.LogError($"AOT code not generated to box {typeof(IReadOnlyObservableVariable<T>).GetPrettifiedName()}. " +
					               $"You must force the compiler to generate a VariableBoxer by using " +
					               $"\"{nameof(AotHelper)}.{nameof(AotHelper.EnsureType)}<{typeof(T).GetPrettifiedName()}>();\" " +
					               $"anywhere in your code.\n" +
					               $"Context: {GetContextPath(context)}", context);
				}
			}

			return result;
		}

		private void RaiseFirstNotification()
		{
			if (boundProperty.HasValue)
			{
				OnBoundPropertyChanged(boundProperty.Value);
			}
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
