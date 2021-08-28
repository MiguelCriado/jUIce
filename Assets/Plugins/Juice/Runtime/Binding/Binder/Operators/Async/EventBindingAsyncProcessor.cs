using System.Threading.Tasks;
using UnityEngine;

namespace Juice
{
	public abstract class EventBindingAsyncProcessor<TFrom, TTo> : IBindingProcessor
	{
		public IViewModel ViewModel { get; }

		protected readonly EventBinding<TFrom> eventBinding;
		protected readonly ObservableEvent<TTo> processedEvent;

		public EventBindingAsyncProcessor(BindingInfo bindingInfo, Component context)
		{
			processedEvent = new ObservableEvent<TTo>();
			ViewModel = new OperatorEventViewModel<TTo>(processedEvent);
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

		protected abstract Task<TTo> ProcessValueAsync(TFrom value);

		protected virtual async void BoundEventRaisedHandler(TFrom eventData)
		{
			TTo result = await ProcessValueAsync(eventData);

			processedEvent.Raise(result);
		}
	}
}