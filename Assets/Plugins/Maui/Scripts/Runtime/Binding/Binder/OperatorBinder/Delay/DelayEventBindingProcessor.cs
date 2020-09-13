using System;
using System.Collections.Generic;
using UnityEngine;

namespace Maui
{
	public class DelayEventBindingProcessor<T> : EventBindingProcessor<T, T>
	{
		private Queue<ActionCommand> actionQueue;
		private float delay;
		
		public DelayEventBindingProcessor(BindingInfo bindingInfo, Component context, float delay)
			: base(bindingInfo, context)
		{
			actionQueue = new Queue<ActionCommand>();
			this.delay = delay;
		}

		public override void Bind()
		{
			LifecycleUtils.OnUpdate += Update;
			base.Bind();
		}

		protected override T ProcessValue(T value)
		{
			LifecycleUtils.OnUpdate -= Update;
			return value;
		}

		protected override void BoundEventRaisedHandler(T eventData)
		{
			EnqueueAction(() => base.BoundEventRaisedHandler(eventData));
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