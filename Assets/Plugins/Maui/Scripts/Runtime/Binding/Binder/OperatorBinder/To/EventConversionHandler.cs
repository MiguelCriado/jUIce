using System;
using UnityEngine;

namespace Maui
{
	public class EventConversionHandler<TFrom, TTo> : ConversionHandler<TFrom, TTo>
	{
		public override IViewModel ViewModel { get; }

		private readonly EventBinding<TFrom> eventBinding;
		private readonly ObservableEvent<TTo> convertedEvent;
		
		public EventConversionHandler(BindingInfo bindingInfo, Component context, Func<TFrom, TTo> conversionFunction)
			: base(bindingInfo, context, conversionFunction)
		{
			convertedEvent = new ObservableEvent<TTo>();
			ViewModel = new OperatorBinderEventViewModel<TTo>(convertedEvent);
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
			convertedEvent.Raise(conversionFunction(eventData));
		}
	}
}