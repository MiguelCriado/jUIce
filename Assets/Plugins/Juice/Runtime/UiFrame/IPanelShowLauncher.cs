using System.Threading.Tasks;

namespace Juice
{
	public interface IPanelShowLauncher
	{
		IPanelShowLauncher WithViewModel(IViewModel viewModel);
		IPanelShowLauncher WithPriority(PanelPriority priority);
		IPanelShowLauncher WithShowTransition(ITransition transition);
		IPanelShowLauncher WithHideTransition(ITransition transition);
		void Execute();
		Task ExecuteAsync();
	}
}
