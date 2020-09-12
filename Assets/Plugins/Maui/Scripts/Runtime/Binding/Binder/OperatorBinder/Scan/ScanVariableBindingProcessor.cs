using System;
using UnityEngine;

namespace Maui
{
	public class ScanVariableBindingProcessor<T> : VariableBindingProcessor<T, T>
	{
		private readonly Func<T, T, T> scanFunction;
		private readonly Func<T> initialAccumulatedValueGetter;

		public ScanVariableBindingProcessor(
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
			processedVariable.Value = initialAccumulatedValueGetter();
			base.Bind();
		}

		protected override T ProcessValue(T value)
		{
			return scanFunction(value, processedVariable.Value);
		}
	}
}