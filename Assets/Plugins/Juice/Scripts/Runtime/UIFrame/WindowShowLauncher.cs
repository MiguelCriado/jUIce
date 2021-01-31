using System;
using System.Threading.Tasks;

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
			ExecuteAsync().RunAndForget();
		}

		public async Task ExecuteAsync()
		{
			priority = null;
			await context.ShowWindow(BuildSettings());
		}

		public void InForeground()
		{
			InForegroundAsync().RunAndForget();
		}

		public async Task InForegroundAsync()
		{
			priority = WindowPriority.ForceForeground;
			await context.ShowWindow(BuildSettings());
		}

		public void Enqueue()
		{
			priority = WindowPriority.Enqueue;
			context.ShowWindow(BuildSettings()).RunAndForget();
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
