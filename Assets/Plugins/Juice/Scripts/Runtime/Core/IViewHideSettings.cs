using System;

namespace Juice
{
	public interface IViewHideSettings
	{
		Type ViewType { get; }
		Transition OutTransition { get; }
	}
}
