using System;

namespace Juice
{
	public interface IViewModelInjector
	{
		Type InjectionType { get; }
		ViewModelComponent Target { get; }
	}
}
