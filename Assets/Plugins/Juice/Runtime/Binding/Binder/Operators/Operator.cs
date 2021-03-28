using System;

namespace Juice
{
	public abstract class Operator : ViewModelComponent, IViewModelInjector
	{
		public Type InjectionType => GetInjectionType();
		public ViewModelComponent Target => this;

		protected abstract Type GetInjectionType();
	}
}
