using System;
using System.Threading.Tasks;
using Juice.Utils;

namespace Juice
{
	public class PanelHideLauncher : IPanelHideLauncher
	{
		private readonly Type panelType;
		private readonly Func<PanelHideSettings, Task> hideCallback;

		private ITransition transition;

		public PanelHideLauncher(Type panelType, Func<PanelHideSettings, Task> hideCallback)
		{
			this.panelType = panelType;
			this.hideCallback = hideCallback;
		}

		public IPanelHideLauncher WithTransition(ITransition transition)
		{
			this.transition = transition;
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
			return new PanelHideSettings(panelType, transition);
		}
	}
}
