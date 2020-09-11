using System;
using UnityEngine;

namespace Maui
{
	public abstract class ConversionHandler<TFrom, TTo>
	{
		public abstract IViewModel ViewModel { get; }
		
		protected Func<TFrom, TTo> conversionFunction;

		protected ConversionHandler(BindingInfo bindingInfo, Component context, Func<TFrom, TTo> conversionFunction)
		{
			this.conversionFunction = conversionFunction;
		}
		
		public abstract void Bind();
		public abstract void Unbind();
	}
}