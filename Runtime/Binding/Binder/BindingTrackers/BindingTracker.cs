using System.Collections.Generic;
using UnityEngine;

namespace Juice
{
	public class BindingTracker
	{
		private readonly Component context;
		private readonly List<Binding> bindings;

		public BindingTracker(Component context)
		{
			this.context = context;
			bindings = new List<Binding>();
		}

		public void Bind()
		{
			foreach (Binding current in bindings)
			{
				current.Bind();
			}
		}

		public void Unbind()
		{
			foreach (Binding current in bindings)
			{
				current.Unbind();
			}
		}

		public VariableBindingSubscriber<T> RegisterVariable<T>(BindingInfo info)
		{
			var binding = new VariableBinding<T>(info, context);
			bindings.Add(binding);
			return new VariableBindingSubscriber<T>(binding);
		}

		public CollectionBindingSubscriber<T> RegisterCollection<T>(BindingInfo info)
		{
			var binding = new CollectionBinding<T>(info, context);
			bindings.Add(binding);
			return new CollectionBindingSubscriber<T>(binding);
		}

		public EventBindingSubscriber RegisterEvent(BindingInfo info)
		{
			var binding = new EventBinding(info, context);
			bindings.Add(binding);
			return new EventBindingSubscriber(binding);
		}

		public EventBindingSubscriber<T> RegisterEvent<T>(BindingInfo info)
		{
			var binding = new EventBinding<T>(info, context);
			bindings.Add(binding);
			return new EventBindingSubscriber<T>(binding);
		}

		public CommandBindingSubscriber RegisterCommand(BindingInfo info)
		{
			var binding = new CommandBinding(info, context);
			bindings.Add(binding);
			return new CommandBindingSubscriber(binding);
		}

		public CommandBindingSubscriber<T> RegisterCommand<T>(BindingInfo info)
		{
			var binding = new CommandBinding<T>(info, context);
			bindings.Add(binding);
			return new CommandBindingSubscriber<T>(binding);
		}
	}
}
