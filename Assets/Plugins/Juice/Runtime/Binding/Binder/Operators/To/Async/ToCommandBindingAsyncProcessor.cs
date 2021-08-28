using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Juice
{
	public class ToCommandBindingAsyncProcessor<TFrom, TTo> : CommandBindingAsyncProcessor<TFrom, TTo>
	{
		private readonly Func<TFrom, Task<TTo>> processFunction;

		public ToCommandBindingAsyncProcessor(BindingInfo bindingInfo, Component context, Func<TFrom, Task<TTo>> processFunction)
			: base(bindingInfo, context)
		{
			this.processFunction = processFunction;
		}

		protected override async Task<TTo> ProcessValueAsync(TFrom value)
		{
			return await processFunction(value);
		}
	}
}