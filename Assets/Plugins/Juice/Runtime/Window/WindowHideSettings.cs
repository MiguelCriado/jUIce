using System;
using System.Collections.Generic;

namespace Juice
{
	public class WindowHideSettings : IViewHideSettings
	{
		public Type ViewType { get; }
		public Dictionary<string, object> Payload { get; }
		public ITransition HideTransition { get; }
		public ITransition ShowTransition { get; }
		public Type DestinationViewType { get; }

		public WindowHideSettings(
			Type viewType,
			Dictionary<string, object> payload,
			ITransition hideTransition,
			ITransition showTransition,
			Type destinationViewType)
		{
			ViewType = viewType;
			Payload = payload;
			HideTransition = hideTransition;
			ShowTransition = showTransition;
			DestinationViewType = destinationViewType;
		}
	}
}
