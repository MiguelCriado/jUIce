namespace Juice
{
	public interface IPanelShowLauncher
	{
		IPanelShowLauncher WithViewModel(IViewModel viewModel);
		IPanelShowLauncher WithShowTransition(ITransition transition);
		IPanelShowLauncher WithHideTransition(ITransition transition);
		void Execute();
	}
}
