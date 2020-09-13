using System;
using System.Collections.Generic;
using UnityEngine;

namespace Maui
{
	public class DelayCommandBindingProcessor<T> : CommandBindingProcessor<T, T>
	{
		private readonly Queue<ActionCommand> actionQueue;
		private readonly float delay;
		
		public DelayCommandBindingProcessor(BindingInfo bindingInfo, Component context, float delay)
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

		public override void Unbind()
		{
			LifecycleUtils.OnUpdate -= Update;
			base.Unbind();
		}

		protected override void ProcessedCommandExecuteRequestedHandler(T parameter)
		{
			EnqueueAction(() => base.ProcessedCommandExecuteRequestedHandler(parameter));
		}

		protected override T ProcessValue(T value)
		{
			return value;
		}
		
		private void Update()
		{
			while (actionQueue.Count > 0 && Time.realtimeSinceStartup >= actionQueue.Peek().DueTime)
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