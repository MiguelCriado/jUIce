using System;

namespace Juice
{
	public class WindowShowSettings : IViewShowSettings
	{
		public Type ViewType { get; }
		public IViewModel ViewModel { get; }
		public Transition InTransition { get; }
		public Transition OutTransition { get; }
		public WindowPriority? Priority { get; }

		public WindowShowSettings(Type windowType, IViewModel viewModel, Transition inTransition, Transition outTransition, WindowPriority? priority)
		{
			ViewType = windowType;
			ViewModel = viewModel;
			InTransition = inTransition;
			OutTransition = outTransition;
			Priority = priority;
		}
	}
}
