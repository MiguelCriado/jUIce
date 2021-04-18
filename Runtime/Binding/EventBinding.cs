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
				boundProperty = BoxEvent(property);
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

		private static IObservableEvent<T> BoxEvent(object eventToBox)
		{
			IObservableEvent<T> result = null;

			Type eventGenericType = eventToBox.GetType().GetGenericClassTowardsRoot();

			if (eventGenericType != null)
			{
				Type exposedType = typeof(T);
				Type boxedType = eventGenericType.GenericTypeArguments[0];
				Type activationType = typeof(EventBoxer<,>).MakeGenericType(exposedType, boxedType);
				result = Activator.CreateInstance(activationType, eventToBox) as IObservableEvent<T>;
			}

			return result;
		}

		private void OnBoundPropertyRaised(T eventData)
		{
			exposedProperty.Raise(eventData);
		}
	}
}
