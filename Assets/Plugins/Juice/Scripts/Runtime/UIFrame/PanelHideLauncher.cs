using System;
using System.Threading.Tasks;

namespace Juice
{
	public class PanelHideLauncher : IPanelHideLauncher
	{
		private readonly UIFrame context;
		private readonly Type panelType;

		private Transition outTransition;

		public PanelHideLauncher(UIFrame context, Type panelType)
		{
			this.context = context;
			this.panelType = panelType;
		}

		public IPanelHideLauncher WithOutTransition(Transition transition)
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
			await context.HidePanel(BuildSettings());
		}

		private PanelHideSettings BuildSettings()
		{
			return new PanelHideSettings(panelType, outTransition);
		}
	}
}
