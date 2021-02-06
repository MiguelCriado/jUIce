using System;
using System.Threading.Tasks;

namespace Juice
{
	public class WindowHideLauncher : IWindowHideLauncher
	{
		private readonly Type windowType;
		private readonly Func<WindowHideSettings, Task> hideCallback;

		private ITransition transition;

		public WindowHideLauncher(Type windowType, Func<WindowHideSettings, Task> hideCallback)
		{
			this.windowType = windowType;
			this.hideCallback = hideCallback;
		}

		public IWindowHideLauncher WithTransition(ITransition transition)
		{
			this.transition = transition;
			return this;
		}

		public void Execute()
		{
			ExecuteAsync().RunAndForget();
		}

		public async Task ExecuteAsync()
		{
			await hideCallback(BuildSettings());
		}

		private WindowHideSettings BuildSettings()
		{
			return new WindowHideSettings(windowType, transition);
		}
	}
}
