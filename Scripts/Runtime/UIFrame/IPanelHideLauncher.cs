using System.Threading.Tasks;

namespace Juice
{
	public interface IPanelHideLauncher
	{
		IPanelHideLauncher WithOutTransition(Transition transition);
		void Execute();
		Task ExecuteAsync();
	}
}
