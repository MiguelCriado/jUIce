using System.Threading.Tasks;

namespace Juice
{
	public interface IPanelHideLauncher
	{
		IPanelHideLauncher WithHideTransition(ITransition transition);
		IPanelHideLauncher WithShowTransition(ITransition transition);
		void Execute();
		Task ExecuteAsync();
	}
}
