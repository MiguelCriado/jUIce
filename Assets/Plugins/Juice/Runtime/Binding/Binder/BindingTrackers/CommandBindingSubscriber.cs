using System;
using UnityEngine.Events;

namespace Juice
{
	public class CommandBindingSubscriber
	{
		private readonly CommandBinding binding;

		public CommandBindingSubscriber(CommandBinding binding)
		{
			this.binding = binding;
		}

		public CommandBindingSubscriber AddExecuteTrigger(UnityEvent eventToTrack)
		{
			eventToTrack.AddListener(() => binding.Property.Execute());
			return this;
		}

		public CommandBindingSubscriber AddExecuteTrigger(Action<Action> eventToTrack)
		{
			eventToTrack.Invoke(() => binding.Property.Execute());
			return this;
		}

		public CommandBindingSubscriber OnCanExecuteChanged(ObservableVariableEventHandler<bool> callback)
		{
			binding.Property.CanExecute.Changed += callback;
			return this;
		}
	}

	public class CommandBindingSubscriber<T>
	{
		private readonly CommandBinding<T> binding;

		public CommandBindingSubscriber(CommandBinding<T> binding)
		{
			this.binding = binding;
		}

		public CommandBindingSubscriber<T> AddExecuteTrigger(UnityEvent<T> eventToTrack)
		{
			eventToTrack.AddListener(value => binding.Property.Execute(value));
			return this;
		}

		public CommandBindingSubscriber<T> AddExecuteTrigger(Action<Action<T>> eventToTrack)
		{
			eventToTrack.Invoke(value => binding.Property.Execute(value));
			return this;
		}

		public CommandBindingSubscriber<T> OnCanExecuteChanged(ObservableVariableEventHandler<bool> callback)
		{
			binding.Property.CanExecute.Changed += callback;
			return this;
		}
	}
}
