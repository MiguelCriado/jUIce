using System;
using UnityEngine;

namespace Maui
{
	public class ScanEventBindingProcessor<T> : EventBindingProcessor<T, T>
	{
		private readonly Func<T, T, T> scanFunction;
		private readonly Func<T> initialAccumulatedValueGetter;
		private T accumulatedValue;

		public ScanEventBindingProcessor(
			BindingInfo bindingInfo,
			Component context,
			Func<T, T, T> scanFunction,
			Func<T> initialAccumulatedValueGetter)
			: base(bindingInfo, context)
		{
			this.scanFunction = scanFunction;
			this.initialAccumulatedValueGetter = initialAccumulatedValueGetter;
		}
		
		public override void Bind()
		{
			accumulatedValue = initialAccumulatedValueGetter();
			eventBinding.Bind();
		}

		protected override T ProcessValue(T value)
		{
			accumulatedValue = scanFunction(value, accumulatedValue);
			return accumulatedValue;
		}
	}
}