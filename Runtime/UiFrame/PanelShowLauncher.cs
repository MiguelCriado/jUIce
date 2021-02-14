using System;
using System.Threading.Tasks;

namespace Juice
{
	public class PanelShowLauncher : IPanelShowLauncher
	{
		private readonly Type panelType;
		private readonly Func<PanelShowSettings, Task> showCallback;

		private IViewModel viewModel;
		private ITransition showTransition;
		private ITransition hideTransition;

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

		public IPanelShowLauncher WithShowTransition(ITransition transition)
		{
			showTransition = transition;
			return this;
		}

		public IPanelShowLauncher WithHideTransition(ITransition transition)
		{
			hideTransition = transition;
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
				showTransition,
				hideTransition);
		}
	}
}
