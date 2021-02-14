using System.Threading.Tasks;

namespace Juice
{
	public interface IWindowHideLauncher
	{
		IWindowHideLauncher WithTransition(ITransition transition);
		void Execute();
		Task ExecuteAsync();
	}
}
