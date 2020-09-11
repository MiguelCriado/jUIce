using System;
using UnityEngine;

namespace Maui
{
	public abstract class BindingProcessor<TFrom, TTo>
	{
		public abstract IViewModel ViewModel { get; }
		
		protected readonly Func<TFrom, TTo> processFunction;

		protected BindingProcessor(BindingInfo bindingInfo, Component context, Func<TFrom, TTo> processFunction)
		{
			this.processFunction = processFunction;
		}
		
		public abstract void Bind();
		public abstract void Unbind();
	}
}