using System;

namespace Juice
{
	public interface IViewHideSettings
	{
		Type ViewType { get; }
		ITransition HideTransition { get; }
		ITransition ShowTransition { get; }
	}
}
