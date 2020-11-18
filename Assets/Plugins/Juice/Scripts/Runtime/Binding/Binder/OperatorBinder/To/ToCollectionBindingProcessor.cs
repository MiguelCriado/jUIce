using System;
using UnityEngine;

namespace Juice
{
	public class ToCollectionBindingProcessor<TFrom, TTo> : CollectionBindingProcessor<TFrom, TTo>
	{
		private readonly Func<TFrom, TTo> processFunction;

		public ToCollectionBindingProcessor(BindingInfo bindingInfo, Component context, Func<TFrom, TTo> processFunction)
			: base(bindingInfo, context)
		{
			this.processFunction = processFunction;
		}

		protected override TTo ProcessValue(TFrom newValue, TFrom oldValue, bool isNewItem)
		{
			return processFunction(newValue);
		}
	}
}