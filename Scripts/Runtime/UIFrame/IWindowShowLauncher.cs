namespace Juice
{
	public interface IWindowShowLauncher
	{
		IWindowShowLauncher WithViewModel(IViewModel viewModel);
		IWindowShowLauncher WithInTransition(Transition transition);
		IWindowShowLauncher WithOutTransition(Transition transition);
		IWindowShowLauncher WithPriority(WindowPriority priority);
		void Execute();
		void InForeground();
		void Enqueue();
	}
}
