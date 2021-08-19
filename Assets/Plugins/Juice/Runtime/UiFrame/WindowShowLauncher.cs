using System;
using System.Threading.Tasks;
using Juice.Utils;

namespace Juice
{
	public class WindowShowLauncher : IWindowShowLauncher
	{
		private readonly Type windowType;
		private readonly Func<WindowShowSettings, Task> showCallback;

		private IViewModel viewModel;
		private ITransition originShowTransition;
		private ITransition originHideTransition;
		private ITransition destinationShowTransition;
		private ITransition destinationHideTransition;
		private WindowPriority? priority;

		public WindowShowLauncher(Type windowType, Func<WindowShowSettings, Task> showCallback)
		{
			this.windowType = windowType;
			this.showCallback = showCallback;
		}

		public IWindowShowLauncher WithViewModel(IViewModel viewModel)
		{
			this.viewModel = viewModel;
			return this;
		}

		public IWindowShowLauncher WithOriginShowTransition(ITransition transition)
		{
			originShowTransition = transition;
			return this;
		}

		public IWindowShowLauncher WithOriginHideTransition(ITransition transition)
		{
			originHideTransition = transition;
			return this;
		}
		
		public IWindowShowLauncher WithDestinationShowTransition(ITransition transition)
		{
			destinationShowTransition = transition;
			return this;
		}

		public IWindowShowLauncher WithDestinationHideTransition(ITransition transition)
		{
			destinationHideTransition = transition;
			return this;
		}

		public IWindowShowLauncher WithPriority(WindowPriority priority)
		{
			this.priority = priority;
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
				originHideTransition,
				destinationShowTransition,
				destinationHideTransition,
				originShowTransition,
				priority);
		}
	}
}
