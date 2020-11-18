using System;

namespace Juice
{
	public abstract class OperatorBinder : ViewModelComponent, IViewModelInjector
	{
		public Type InjectionType => GetInjectionType();
		public ViewModelComponent Target => this;
		
		protected abstract Type GetInjectionType();
	}
}