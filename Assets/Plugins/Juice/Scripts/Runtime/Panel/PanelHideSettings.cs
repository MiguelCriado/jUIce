using System;

namespace Juice
{
	public class PanelHideSettings : IViewHideSettings
	{
		public Type ViewType { get; }
		public Transition OutTransition { get; }

		public PanelHideSettings(Type viewType, Transition outTransition)
		{
			ViewType = viewType;
			OutTransition = outTransition;
		}
	}
}
