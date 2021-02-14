using System;

namespace Juice
{
	public interface IViewShowSettings
	{
		Type ViewType { get; }
		IViewModel ViewModel { get; }
		ITransition ShowTransition { get; }
		ITransition HideTransition { get; }
	}
}
