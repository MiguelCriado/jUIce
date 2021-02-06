using System;

namespace Juice
{
	public class PanelHideSettings : IViewHideSettings
	{
		public Type ViewType { get; }
		public ITransition Transition { get; }

		public PanelHideSettings(Type viewType, ITransition outTransition)
		{
			ViewType = viewType;
			Transition = outTransition;
		}
	}
}
