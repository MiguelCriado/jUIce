using System;

namespace Juice
{
	public interface IViewHideSettings
	{
		Type ViewType { get; }
		ITransition Transition { get; }
	}
}
