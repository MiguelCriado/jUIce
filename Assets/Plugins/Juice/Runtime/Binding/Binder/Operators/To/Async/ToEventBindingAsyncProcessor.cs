using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Juice
{
	public class ToEventBindingAsyncProcessor<TFrom, TTo> : EventBindingAsyncProcessor<TFrom, TTo>
	{
		private readonly Func<TFrom, Task<TTo>> processFunction;

		public ToEventBindingAsyncProcessor(BindingInfo bindingInfo, Component context, Func<TFrom, Task<TTo>> processFunction)
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