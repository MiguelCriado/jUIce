using System;
using System.Threading.Tasks;

namespace Juice
{
	public interface IWindowShowLauncher
	{
		IWindowShowLauncher WithViewModel(IViewModel viewModel);
		IWindowShowLauncher AddPayload(string key, object value);
		IWindowShowLauncher WithOriginHideTransition(ITransition transition);
		IWindowShowLauncher WithDestinationShowTransition(ITransition transition);
		IWindowShowLauncher WithDestinationHideTransition(ITransition transition);
		IWindowShowLauncher WithOriginShowTransition(ITransition transition);
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
