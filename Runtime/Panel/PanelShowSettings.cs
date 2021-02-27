using System;

namespace Juice
{
	public class PanelShowSettings : IViewShowSettings
	{
		public Type ViewType { get; }
		public IViewModel ViewModel { get; }
		public PanelPriority? Priority { get; }
		public ITransition ShowTransition { get; }
		public ITransition HideTransition { get; }

		public PanelShowSettings(Type viewType, IViewModel viewModel, PanelPriority? priority,ITransition showTransition, ITransition hideTransition)
		{
			ViewType = viewType;
			ViewModel = viewModel;
			Priority = priority;
			ShowTransition = showTransition;
			HideTransition = hideTransition;
		}
	}
}
