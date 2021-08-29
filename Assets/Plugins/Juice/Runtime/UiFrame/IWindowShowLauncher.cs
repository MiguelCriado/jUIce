using System;
using System.Threading.Tasks;

namespace Juice
{
	public interface IWindowShowLauncher
	{
		IWindowShowLauncher WithViewModel(IViewModel viewModel);
		IWindowShowLauncher AddPayload(string key, object value);
		IWindowShowLauncher WithShowTransition(ITransition transition);
		IWindowShowLauncher WithHideTransition(ITransition transition);
		IWindowShowLauncher WithPriority(WindowPriority priority);
		IWindowShowLauncher WithBackDestination(Type viewType);
		IWindowShowLauncher WithStubViewType(Type viewType);
		void Execute();
		Task ExecuteAsync();
		void InForeground();
		Task InForegroundAsync();
		void Enqueue();
	}
}
