using System;
using UnityEngine;

namespace Maui
{
	public class EventBindingProcessor<TFrom, TTo> : BindingProcessor<TFrom, TTo>
	{
		public override IViewModel ViewModel { get; }

		private readonly EventBinding<TFrom> eventBinding;
		private readonly ObservableEvent<TTo> processedEvent;
		
		public EventBindingProcessor(BindingInfo bindingInfo, Component context, Func<TFrom, TTo> processFunction)
			: base(bindingInfo, context, processFunction)
		{
			processedEvent = new ObservableEvent<TTo>();
			ViewModel = new OperatorBinderEventViewModel<TTo>(processedEvent);
			eventBinding = new EventBinding<TFrom>(bindingInfo, context);
			eventBinding.Property.Raised += BoundEventRaisedHandler; 
		}

		public override void Bind()
		{
			eventBinding.Bind();
		}

		public override void Unbind()
		{
			eventBinding.Unbind();
		}
		
		private void BoundEventRaisedHandler(TFrom eventData)
		{
			processedEvent.Raise(processFunction(eventData));
		}
	}
}