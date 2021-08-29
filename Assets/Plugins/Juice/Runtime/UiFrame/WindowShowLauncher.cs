using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Juice.Utils;

namespace Juice
{
	public class WindowShowLauncher : IWindowShowLauncher
	{
		private readonly Type windowType;
		private readonly Func<WindowShowSettings, Task> showCallback;
		private readonly Dictionary<string, object> payload;

		private IViewModel viewModel;
		private ITransition showTransition;
		private ITransition hideTransition;
		private WindowPriority? priority;
		private Type backDestinationType;
		private Type stubViewType;

		public WindowShowLauncher(Type windowType, Func<WindowShowSettings, Task> showCallback)
		{
			this.windowType = windowType;
			this.showCallback = showCallback;
			payload = new Dictionary<string, object>();
		}

		public IWindowShowLauncher WithViewModel(IViewModel viewModel)
		{
			this.viewModel = viewModel;
			return this;
		}
		
		public IWindowShowLauncher AddPayload(string key, object value)
		{
			payload[key] = value;
			return this;
		}

		public IWindowShowLauncher WithShowTransition(ITransition transition)
		{
			showTransition = transition;
			return this;
		}

		public IWindowShowLauncher WithHideTransition(ITransition transition)
		{
			hideTransition = transition;
			return this;
		}

		public IWindowShowLauncher WithPriority(WindowPriority priority)
		{
			this.priority = priority;
			return this;
		}

		public IWindowShowLauncher WithBackDestination(Type backDestinationType)
		{
			this.backDestinationType = backDestinationType;
			return this;
		}

		public IWindowShowLauncher WithStubViewType(Type stubViewType)
		{
			this.stubViewType = stubViewType;
			return this;
		}

		public void Execute()
		{
			ExecuteAsync().RunAndForget();
		}

		public async Task ExecuteAsync()
		{
			priority = null;
			await showCallback(BuildSettings());
		}

		public void InForeground()
		{
			InForegroundAsync().RunAndForget();
		}

		public async Task InForegroundAsync()
		{
			priority = WindowPriority.ForceForeground;
			await showCallback(BuildSettings());
		}

		public void Enqueue()
		{
			priority = WindowPriority.Enqueue;
			showCallback(BuildSettings()).RunAndForget();
		}

		private WindowShowSettings BuildSettings()
		{
			return new WindowShowSettings(
				windowType,
				viewModel,
				payload,
				showTransition,
				hideTransition,
				priority,
				backDestinationType,
				stubViewType);
		}
	}
}
