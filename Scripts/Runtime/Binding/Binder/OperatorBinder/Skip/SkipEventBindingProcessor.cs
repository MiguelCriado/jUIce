using UnityEngine;

namespace Juice
{
	public class SkipEventBindingProcessor<T> : EventBindingProcessor<T, T>
	{
		private readonly int skipAmount;
		private int valueReceivedCount;
		
		public SkipEventBindingProcessor(BindingInfo bindingInfo, Component context, int skipAmount)
			: base(bindingInfo, context)
		{
			this.skipAmount = skipAmount;
			valueReceivedCount = 0;
		}

		public override void Bind()
		{
			valueReceivedCount = 0;
			
			base.Bind();
		}

		protected override void BoundEventRaisedHandler(T eventData)
		{
			if (valueReceivedCount >= skipAmount)
			{
				base.BoundEventRaisedHandler(eventData);
			}
		}

		protected override T ProcessValue(T value)
		{
			return value;
		}
	}
}