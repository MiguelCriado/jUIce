using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Juice
{
	public class ToCollectionBindingAsyncProcessor<TFrom, TTo> : CollectionBindingAsyncProcessor<TFrom, TTo>
	{
		private readonly Func<TFrom, Task<TTo>> processFunction;

		public ToCollectionBindingAsyncProcessor(BindingInfo bindingInfo, Component context, Func<TFrom, Task<TTo>> processFunction)
			: base(bindingInfo, context)
		{
			this.processFunction = processFunction;
		}

		protected override async Task<TTo> ProcessValueAsync(TFrom newValue, TFrom oldValue, bool isNewItem)
		{
			return await processFunction(newValue);
		}
	}
}