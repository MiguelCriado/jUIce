using System;
using System.Collections.Generic;
using UnityEngine;

namespace Maui
{
	public class DelayVariableBindingProcessor<T> : VariableBindingProcessor<T, T>
	{
		private Queue<ActionCommand> actionQueue;
		private float delay;
		
		public DelayVariableBindingProcessor(BindingInfo bindingInfo, Component context, float delay)
			: base(bindingInfo, context)
		{
			actionQueue = new Queue<ActionCommand>();
			this.delay = delay;
		}

		public override void Bind()
		{
			actionQueue.Clear();
			LifecycleUtils.OnUpdate += Update;
			base.Bind();
		}

		public override void Unbind()
		{
			LifecycleUtils.OnUpdate -= Update;
			base.Unbind();
		}

		protected override void BoundVariableChangedHandler(T newValue)
		{
			EnqueueAction(() => base.BoundVariableChangedHandler(newValue));
		}

		protected override T ProcessValue(T value)
		{
			return value;
		}
		
		private void Update()
		{
			while (actionQueue.Count > 0 && actionQueue.Peek().DueTime >= Time.realtimeSinceStartup)
			{
				actionQueue.Dequeue().Execute();
			}
		}
		
		private void EnqueueAction(Action action)
		{
			actionQueue.Enqueue(new ActionCommand(action, Time.realtimeSinceStartup + delay));
		}
	}
}