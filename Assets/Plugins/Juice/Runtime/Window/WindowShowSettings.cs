using System;

namespace Juice
{
	public class WindowShowSettings : IViewShowSettings
	{
		public Type ViewType { get; }
		public IViewModel ViewModel { get; }
		public ITransition HideTransition { get; }
		public ITransition DestinationShowTransition { get; }
		public ITransition DestinationHideTransition { get; }
		public ITransition ShowTransition { get; }
		public WindowPriority? Priority { get; }

		public WindowShowSettings(
			Type windowType,
			IViewModel viewModel,
			ITransition originHideTransition,
			ITransition destinationShowTransition,
			ITransition destinationHideTransition,
			ITransition originShowTransition,
			WindowPriority? priority)
		{
			ViewType = windowType;
			ViewModel = viewModel;
			HideTransition = originHideTransition;
			DestinationShowTransition = destinationShowTransition;
			DestinationHideTransition = destinationHideTransition;
			ShowTransition = originShowTransition;
			Priority = priority;
		}
	}
}
