using UnityEngine;

namespace Juice
{
	public class SkipCommandBindingProcessor<T> : CommandBindingProcessor<T, T>
	{
		private readonly int skipAmount;
		private int valueReceivedCount;
		
		public SkipCommandBindingProcessor(BindingInfo bindingInfo, Component context, int skipAmount)
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

		protected override void ProcessedCommandExecuteRequestedHandler(T parameter)
		{
			valueReceivedCount++;

			if (valueReceivedCount >= skipAmount)
			{
				base.ProcessedCommandExecuteRequestedHandler(parameter);
			}
		}

		protected override T ProcessValue(T value)
		{
			return value;
		}
	}
}