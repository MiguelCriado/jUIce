using System.Threading.Tasks;

namespace Juice
{
	public interface IPanelHideLauncher
	{
		IPanelHideLauncher WithTransition(ITransition transition);
		void Execute();
		Task ExecuteAsync();
	}
}
