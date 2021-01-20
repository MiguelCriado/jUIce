using System;

namespace Juice
{
	public class PanelShowSettings : IViewShowSettings
	{
		public Type ViewType { get; }
		public IViewModel ViewModel { get; }
		public Transition InTransition { get; }
		public Transition OutTransition { get; }

		public PanelShowSettings(Type viewType, IViewModel viewModel, Transition inTransition, Transition outTransition)
		{
			ViewType = viewType;
			ViewModel = viewModel;
			InTransition = inTransition;
			OutTransition = outTransition;
		}
	}
}
