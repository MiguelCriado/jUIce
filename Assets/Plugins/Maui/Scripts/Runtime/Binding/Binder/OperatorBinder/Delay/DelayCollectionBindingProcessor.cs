using System;
using System.Collections.Generic;
using UnityEngine;

namespace Maui
{
	public class DelayCollectionBindingProcessor<T> : CollectionBindingProcessor<T, T>
	{
		private readonly Queue<ActionCommand> actionQueue;
		private readonly float delay;
		
		public DelayCollectionBindingProcessor(BindingInfo bindingInfo, Component context, float delay)
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

		protected override void BoundCollectionResetHandler()
		{
			EnqueueAction(() => base.BoundCollectionResetHandler());
		}

		protected override void BoundCollectionItemAddedHandler(int index, T value)
		{
			EnqueueAction(() => base.BoundCollectionItemAddedHandler(index, value));
		}

		protected override void BoundCollectionItemMovedHandler(int oldIndex, int newIndex, T value)
		{
			EnqueueAction(() => base.BoundCollectionItemMovedHandler(oldIndex, newIndex, value));
		}

		protected override void BoundCollectionItemRemovedHandler(int index, T value)
		{
			EnqueueAction(() => base.BoundCollectionItemRemovedHandler(index, value));
		}

		protected override void BoundCollectionItemReplacedHandler(int index, T oldValue, T newValue)
		{
			EnqueueAction(() => base.BoundCollectionItemReplacedHandler(index, oldValue, newValue));
		}

		protected override T ProcessValue(T newValue, T oldValue, bool isNewItem)
		{
			return newValue;
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