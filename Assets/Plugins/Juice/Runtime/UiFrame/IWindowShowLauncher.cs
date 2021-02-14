using System.Threading.Tasks;

namespace Juice
{
	public interface IWindowShowLauncher
	{
		IWindowShowLauncher WithViewModel(IViewModel viewModel);
		IWindowShowLauncher WithShowTransition(ITransition transition);
		IWindowShowLauncher WithHideTransition(ITransition transition);
		IWindowShowLauncher WithPriority(WindowPriority priority);
		void Execute();
		Task ExecuteAsync();
		void InForeground();
		Task InForegroundAsync();
		void Enqueue();
	}
}
