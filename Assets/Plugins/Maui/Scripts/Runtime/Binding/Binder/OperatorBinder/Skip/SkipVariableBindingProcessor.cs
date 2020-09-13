using UnityEngine;

namespace Maui
{
	public class SkipVariableBindingProcessor<T> : VariableBindingProcessor<T, T>
	{
		private readonly int skipAmount;
		private int valueReceivedCount;
		
		public SkipVariableBindingProcessor(BindingInfo bindingInfo, Component context, int skipAmount)
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

		protected override void BoundVariableChangedHandler(T newValue)
		{
			if (valueReceivedCount >= skipAmount)
			{
				base.BoundVariableChangedHandler(newValue);
			}

			valueReceivedCount++;
		}

		protected override T ProcessValue(T value)
		{
			return value;
		}
	}
}