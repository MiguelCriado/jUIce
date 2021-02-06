using System;

namespace Juice
{
	public class WindowShowSettings : IViewShowSettings
	{
		public Type ViewType { get; }
		public IViewModel ViewModel { get; }
		public ITransition ShowTransition { get; }
		public ITransition HideTransition { get; }
		public WindowPriority? Priority { get; }

		public WindowShowSettings(Type windowType, IViewModel viewModel, ITransition showTransition, ITransition hideTransition, WindowPriority? priority)
		{
			ViewType = windowType;
			ViewModel = viewModel;
			ShowTransition = showTransition;
			HideTransition = hideTransition;
			Priority = priority;
		}
	}
}
