using System;

namespace Maui
{
	public interface IViewModelInjector<T> where T : IViewModel
	{
		Type ViewModelType { get; }
	}
}
