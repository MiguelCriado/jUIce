using System;

namespace Juice
{
	public class WindowShowLauncher : IWindowShowLauncher
	{
		private readonly UIFrame context;
		private readonly Type windowType;

		private IViewModel viewModel;
		private Transition inTransition;
		private Transition outTransition;
		private WindowPriority? priority;

		public WindowShowLauncher(UIFrame context, Type windowType)
		{
			this.context = context;
			this.windowType = windowType;
		}

		public IWindowShowLauncher WithViewModel(IViewModel viewModel)
		{
			this.viewModel = viewModel;
			return this;
		}

		public IWindowShowLauncher WithInTransition(Transition transition)
		{
			inTransition = transition;
			return this;
		}

		public IWindowShowLauncher WithOutTransition(Transition transition)
		{
			outTransition = transition;
			return this;
		}

		public IWindowShowLauncher WithPriority(WindowPriority priority)
		{
			this.priority = priority;
			return this;
		}

		public void Execute()
		{
			priority = null;
			context.ShowWindow(BuildSettings());
		}

		public void InForeground()
		{
			priority = WindowPriority.ForceForeground;
			context.ShowWindow(BuildSettings());
		}

		public void Enqueue()
		{
			priority = WindowPriority.Enqueue;
			context.ShowWindow(BuildSettings());
		}

		private WindowShowSettings BuildSettings()
		{
			return new WindowShowSettings(
				windowType,
				viewModel,
				inTransition,
				outTransition,
				priority);
		}
	}
}
