using System;
using UnityEngine;

namespace Maui
{
	public class ToVariableBindingProcessor<TFrom, TTo> : VariableBindingProcessor<TFrom, TTo>
	{
		private readonly Func<TFrom, TTo> processFunction;
		
		public ToVariableBindingProcessor(BindingInfo bindingInfo, Component context, Func<TFrom, TTo> processFunction)
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