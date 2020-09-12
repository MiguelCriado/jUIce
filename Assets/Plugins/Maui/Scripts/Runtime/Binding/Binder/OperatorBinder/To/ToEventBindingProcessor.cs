using System;
using UnityEngine;

namespace Maui
{
	public class ToEventBindingProcessor<TFrom, TTo> : EventBindingProcessor<TFrom, TTo>
	{
		private readonly Func<TFrom, TTo> processFunction;

		public ToEventBindingProcessor(BindingInfo bindingInfo, Component context, Func<TFrom, TTo> processFunction)
			: base(bindingInfo, context)
		{
			this.processFunction = processFunction;
		}

		protected override TTo ProcessValue(TFrom value)
		{
			return processFunction(value);
		}
	}
}