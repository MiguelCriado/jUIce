using System;

namespace Maui
{
	public interface IViewModelInjector
	{
		Type InjectionType { get; }
		ViewModelComponent Target { get; }
	}
}
