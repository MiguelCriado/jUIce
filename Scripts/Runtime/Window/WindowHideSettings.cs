using System;

namespace Juice
{
	public class WindowHideSettings : IViewHideSettings
	{
		public Type ViewType { get; }
		public Transition OutTransition { get; }

		public WindowHideSettings(Type viewType, Transition outTransition)
		{
			ViewType = viewType;
			OutTransition = outTransition;
		}
	}
}
