using System;
using System.Threading.Tasks;

namespace Juice
{
	public class PanelShowLauncher : IPanelShowLauncher
	{
		private readonly UIFrame context;
		private readonly Type panelType;

		private IViewModel viewModel;
		private Transition inTransition;
		private Transition outTransition;

		public PanelShowLauncher(UIFrame context, Type panelType)
		{
			this.context = context;
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
			context.ShowPanel(BuildSettings()).RunAndForget();
		}

		public async Task ExecuteAsync()
		{
			await context.ShowPanel(BuildSettings());
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
