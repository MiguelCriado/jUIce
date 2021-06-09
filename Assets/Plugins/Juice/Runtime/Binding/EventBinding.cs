using System;
using Juice.Utils;
using UnityEngine;

namespace Juice
{
	public class EventBinding : Binding
	{
		public override bool IsBound => boundProperty != null;

		public IObservableEvent Property => exposedProperty;

		private readonly ObservableEvent exposedProperty;
		private IObservableEvent boundProperty;

		public EventBinding(BindingInfo bindingInfo, Component context) : base(bindingInfo, context)
		{
			exposedProperty = new ObservableEvent();
		}

		protected override Type GetBindingType()
		{
			return typeof(IObservableEvent);
		}

		protected override void BindProperty(object property)
		{
			boundProperty = property as IObservableEvent;

			if (boundProperty != null)
			{
				boundProperty.Raised += OnBoundPropertyRaised;
			}
			else
			{
				Debug.LogError($"Property type ({property.GetType()}) different from expected type {typeof(IObservableEvent)}", context);
			}
		}

		protected override void UnbindProperty()
		{
			if (boundProperty != null)
			{
				boundProperty.Raised -= OnBoundPropertyRaised;
				boundProperty = null;
			}
		}

		private void OnBoundPropertyRaised()
		{
			exposedProperty.Raise();
		}
	}

	public class EventBinding<T> : Binding
	{
		public override bool IsBound => boundProperty != null;
		public IObservableEvent<T> Property => exposedProperty;

		private readonly ObservableEvent<T> exposedProperty;
		private IObservableEvent<T> boundProperty;

		public EventBinding(BindingInfo bindingInfo, Component context) : base(bindingInfo, context)
		{
			exposedProperty = new ObservableEvent<T>();
		}

		protected override Type GetBindingType()
		{
			return typeof(IObservableEvent<T>);
		}

		protected override void BindProperty(object property)
		{
			boundProperty = property as IObservableEvent<T>;

			if (boundProperty == null)
			{
				boundProperty = BoxEvent(property, context);
			}

			if (boundProperty != null)
			{
				boundProperty.Raised += OnBoundPropertyRaised;
			}
			else
			{
				Debug.LogError($"Property type ({property.GetType()}) different from expected type {typeof(IObservableEvent<T>)}", context);
			}
		}

		protected override void UnbindProperty()
		{
			if (boundProperty != null)
			{
				boundProperty.Raised -= OnBoundPropertyRaised;
				boundProperty = null;
			}
		}

		private static IObservableEvent<T> BoxEvent(object eventToBox, Component context)
		{
			IObservableEvent<T> result = null;

			Type eventGenericType = eventToBox.GetType().GetGenericTypeTowardsRoot();

			if (eventGenericType != null)
			{
				try
				{
					Type exposedType = typeof(T);
					Type boxedType = eventGenericType.GenericTypeArguments[0];
					Type activationType = typeof(EventBoxer<,>).MakeGenericType(exposedType, boxedType);
					result = Activator.CreateInstance(activationType, eventToBox) as IObservableEvent<T>;
				}
#pragma warning disable 618
				catch (ExecutionEngineException)
#pragma warning restore 618
				{
					Debug.LogError($"AOT code not generated to box {typeof(IObservableEvent<T>).GetPrettifiedName()}. " +
					               $"You must force the compiler to generate an EventBoxer by using " +
					               $"\"{nameof(AotHelper)}.{nameof(AotHelper.EnsureType)}<{typeof(T).GetPrettifiedName()}>();\" " +
					               $"anywhere in your code.\n" +
					               $"Context: {GetContextPath(context)}", context);
				}
			}

			return result;
		}

		private void OnBoundPropertyRaised(T eventData)
		{
			exposedProperty.Raise(eventData);
		}
	}
}
