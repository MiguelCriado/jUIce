using System;

namespace Juice
{
	public class WindowHideSettings : IViewHideSettings
	{
		public Type ViewType { get; }
		public ITransition Transition { get; }

		public WindowHideSettings(Type viewType, ITransition transition)
		{
			ViewType = viewType;
			Transition = transition;
		}
	}
}
