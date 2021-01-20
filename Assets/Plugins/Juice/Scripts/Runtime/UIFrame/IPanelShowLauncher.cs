namespace Juice
{
	public interface IPanelShowLauncher
	{
		IPanelShowLauncher WithViewModel(IViewModel viewModel);
		IPanelShowLauncher WithInTransition(Transition transition);
		IPanelShowLauncher WithOutTransition(Transition transition);
		void Execute();
	}
}
