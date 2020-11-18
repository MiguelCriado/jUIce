using System;
using UnityEngine;

namespace Juice
{
	public class ToCommandBindingProcessor<TFrom, TTo> : CommandBindingProcessor<TFrom, TTo>
	{
		private readonly Func<TFrom, TTo> processFunction;

		public ToCommandBindingProcessor(BindingInfo bindingInfo, Component context, Func<TFrom, TTo> processFunction)
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