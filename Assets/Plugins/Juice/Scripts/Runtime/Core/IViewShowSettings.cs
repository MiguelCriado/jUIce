using System;

namespace Juice
{
	public interface IViewShowSettings
	{
		Type ViewType { get; }
		IViewModel ViewModel { get; }
		Transition InTransition { get; }
		Transition OutTransition { get; }
	}
}
