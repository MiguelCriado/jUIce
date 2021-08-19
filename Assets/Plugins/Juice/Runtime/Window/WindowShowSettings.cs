using System;
using System.Collections.Generic;

namespace Juice
{
	public class WindowShowSettings : IViewShowSettings
	{
		public Type ViewType { get; }
		public IViewModel ViewModel { get; set; }
		public Dictionary<string, object> Payload { get; }
		public ITransition HideTransition { get; }
		public ITransition DestinationShowTransition { get; }
		public ITransition DestinationHideTransition { get; }
		public ITransition ShowTransition { get; }
		public WindowPriority? Priority { get; }

		public WindowShowSettings(
			Type windowType,
			IViewModel viewModel,
			Dictionary<string, object> payload,
			ITransition originHideTransition,
			ITransition destinationShowTransition,
			ITransition destinationHideTransition,
			ITransition originShowTransition,
			WindowPriority? priority)
		{
			ViewType = windowType;
			ViewModel = viewModel;
			Payload = payload;
			HideTransition = originHideTransition;
			DestinationShowTransition = destinationShowTransition;
			DestinationHideTransition = destinationHideTransition;
			ShowTransition = originShowTransition;
			Priority = priority;
		}
	}
}
