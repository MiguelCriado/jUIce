using System;
using System.Threading.Tasks;

namespace Juice
{
	public class WindowHideLauncher : IWindowHideLauncher
	{
		private readonly UIFrame context;
		private readonly Type windowType;

		private Transition outTransition;

		public WindowHideLauncher(UIFrame context, Type windowType)
		{
			this.context = context;
			this.windowType = windowType;
		}

		public IWindowHideLauncher WithOutTransition(Transition transition)
		{
			outTransition = transition;
			return this;
		}

		public void Execute()
		{
			ExecuteAsync().RunAndForget();
		}

		public async Task ExecuteAsync()
		{
			await context.HideWindow(BuildSettings());
		}

		private WindowHideSettings BuildSettings()
		{
			return new WindowHideSettings(windowType, outTransition);
		}
	}
}
