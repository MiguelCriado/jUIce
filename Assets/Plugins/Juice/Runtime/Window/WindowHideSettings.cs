using System;
using System.Collections.Generic;

namespace Juice
{
	public class WindowHideSettings : IViewHideSettings
	{
		public Type ViewType { get; }
		public Dictionary<string, object> Payload { get; }
		public ITransition Transition { get; }
		public Type DestinationViewType { get; }

		public WindowHideSettings(
			Type viewType,
			Dictionary<string, object> payload,
			ITransition transition,
			Type destinationViewType)
		{
			ViewType = viewType;
			Payload = payload;
			Transition = transition;
			DestinationViewType = destinationViewType;
		}
	}
}
