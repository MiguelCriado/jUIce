using UnityEngine;

namespace Maui
{
	public abstract class EventBindingProcessor<TFrom, TTo> : IBindingProcessor
	{
		public IViewModel ViewModel { get; }

		protected readonly EventBinding<TFrom> eventBinding;
		protected readonly ObservableEvent<TTo> processedEvent;
		
		public EventBindingProcessor(BindingInfo bindingInfo, Component context)
		{
			processedEvent = new ObservableEvent<TTo>();
			ViewModel = new OperatorBinderEventViewModel<TTo>(processedEvent);
			eventBinding = new EventBinding<TFrom>(bindingInfo, context);
			eventBinding.Property.Raised += BoundEventRaisedHandler; 
		}

		public virtual void Bind()
		{
			eventBinding.Bind();
		}

		public virtual void Unbind()
		{
			eventBinding.Unbind();
		}

		protected abstract TTo ProcessValue(TFrom value);
		
		protected virtual void BoundEventRaisedHandler(TFrom eventData)
		{
			processedEvent.Raise(ProcessValue(eventData));
		}
	}
}