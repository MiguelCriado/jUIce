using System;
using System.Threading.Tasks;
using Juice.Utils;

namespace Juice
{
	public class PanelHideLauncher : IPanelHideLauncher
	{
		private readonly Type panelType;
		private readonly Func<PanelHideSettings, Task> hideCallback;

		private ITransition hideTransition;
		private ITransition showTransition;

		public PanelHideLauncher(Type panelType, Func<PanelHideSettings, Task> hideCallback)
		{
			this.panelType = panelType;
			this.hideCallback = hideCallback;
		}

		public IPanelHideLauncher WithHideTransition(ITransition hideTransition)
		{
			this.hideTransition = hideTransition;
			return this;
		}

		public IPanelHideLauncher WithShowTransition(ITransition showTransition)
		{
			this.showTransition = showTransition;
			return this;
		}

		public void Execute()
		{
			ExecuteAsync().RunAndForget();
		}

		public async Task ExecuteAsync()
		{
			await hideCallback(BuildSettings());
		}

		private PanelHideSettings BuildSettings()
		{
			return new PanelHideSettings(panelType, hideTransition, showTransition);
		}
	}
}
