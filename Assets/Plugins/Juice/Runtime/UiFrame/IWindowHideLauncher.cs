using System;
using System.Threading.Tasks;

namespace Juice
{
	public interface IWindowHideLauncher
	{
		IWindowHideLauncher WithHideTransition(ITransition transition);
		IWindowHideLauncher WithShowTransition(ITransition transition);
		IWindowHideLauncher WithDestination(Type destination);
		IWindowHideLauncher AddPayload(string key, object value);
		void Execute();
		Task ExecuteAsync();
	}
}
