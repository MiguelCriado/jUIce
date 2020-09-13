using System;

namespace Maui
{
	public abstract class OperatorBinder : ViewModelComponent, IViewModelInjector
	{
		public Type InjectionType => GetInjectionType();
		public ViewModelComponent Target => this;
		
		protected abstract Type GetInjectionType();

	}
}