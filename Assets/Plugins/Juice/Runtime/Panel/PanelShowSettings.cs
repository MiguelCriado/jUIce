using System;

namespace Juice
{
	public class PanelShowSettings : IViewShowSettings
	{
		public Type ViewType { get; }
		public IViewModel ViewModel { get; }
		public ITransition ShowTransition { get; }
		public ITransition HideTransition { get; }

		public PanelShowSettings(Type viewType, IViewModel viewModel, ITransition showTransition, ITransition hideTransition)
		{
			ViewType = viewType;
			ViewModel = viewModel;
			ShowTransition = showTransition;
			HideTransition = hideTransition;
		}
	}
}
