using System.Threading.Tasks;

namespace Juice
{
	public interface IWindowHideLauncher
	{
		IWindowHideLauncher WithOutTransition(Transition transition);
		void Execute();
		Task ExecuteAsync();
	}
}
