using System;
using System.Threading.Tasks;

namespace Juice
{
	public class PanelShowLauncher : IPanelShowLauncher
	{
		private readonly Type panelType;
		private readonly Func<PanelShowSettings, Task> showCallback;

		private IViewModel viewModel;
		private Transition inTransition;
		private Transition outTransition;

		public PanelShowLauncher(Type panelType, Func<PanelShowSettings, Task> showCallback)
		{
			this.showCallback = showCallback;
			this.panelType = panelType;
		}

		public IPanelShowLauncher WithViewModel(IViewModel viewModel)
		{
			this.viewModel = viewModel;
			return this;
		}

		public IPanelShowLauncher WithInTransition(Transition transition)
		{
			inTransition = transition;
			return this;
		}

		public IPanelShowLauncher WithOutTransition(Transition transition)
		{
			outTransition = transition;
			return this;
		}

		public void Execute()
		{
			ExecuteAsync().RunAndForget();
		}

		public async Task ExecuteAsync()
		{
			await showCallback(BuildSettings());
		}

		private PanelShowSettings BuildSettings()
		{
			return new PanelShowSettings(
				panelType,
				viewModel,
				inTransition,
				outTransition);
		}
	}
}
