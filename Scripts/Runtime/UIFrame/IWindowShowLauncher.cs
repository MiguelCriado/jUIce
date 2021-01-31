using System.Threading.Tasks;

namespace Juice
{
	public interface IWindowShowLauncher
	{
		IWindowShowLauncher WithViewModel(IViewModel viewModel);
		IWindowShowLauncher WithInTransition(Transition transition);
		IWindowShowLauncher WithOutTransition(Transition transition);
		IWindowShowLauncher WithPriority(WindowPriority priority);
		void Execute();
		Task ExecuteAsync();
		void InForeground();
		Task InForegroundAsync();
		void Enqueue();
	}
}
