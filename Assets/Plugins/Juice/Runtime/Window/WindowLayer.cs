using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Juice.Utils;
using UnityEngine;

namespace Juice
{
	public class WindowLayer : Layer<IWindow, WindowShowSettings, WindowHideSettings>
	{
		public delegate void WindowChangeHandler(IWindow oldWindow, IWindow newWindow, bool fromBack);

		public event WindowChangeHandler CurrentWindowChanged;

		public IEnumerable<IWindow> CurrentPath => GetCurrentPath();
		public IWindow CurrentWindow => currentWindow;

		[SerializeField] private WindowParaLayer priorityParaLayer = null;

		private readonly Queue<WindowHistoryEntry> windowQueue = new Queue<WindowHistoryEntry>();
		private readonly Stack<WindowHistoryEntry> windowHistory = new Stack<WindowHistoryEntry>();
		private List<IWindow> currentPath = new List<IWindow>();
		private IWindow currentWindow;
		private bool currentPathIsDirty;

		protected virtual void OnEnable()
		{
			if (priorityParaLayer != null)
			{
				priorityParaLayer.BackgroundClicked -= OnPopupsBackgroundClicked;
				priorityParaLayer.BackgroundClicked += OnPopupsBackgroundClicked;
			}
		}

		internal void SetPriorityLayer(WindowParaLayer priorityParaLayer)
		{
			this.priorityParaLayer = priorityParaLayer;

			priorityParaLayer.BackgroundClicked -= OnPopupsBackgroundClicked;
			priorityParaLayer.BackgroundClicked += OnPopupsBackgroundClicked;
		}

		public WindowLayerState GetCurrentState()
		{
			IEnumerable<WindowStateEntry> queueState = windowQueue.ToArray().Select(x => new WindowStateEntry(x.View, x.Settings, x.View.IsVisible));
			IEnumerable<WindowStateEntry> historyState = windowHistory.ToArray().Reverse().Select(x => new WindowStateEntry(x.View, x.Settings, x.View.IsVisible));
			return new WindowLayerState(queueState, historyState);
		}

		public void SetState(WindowLayerState state)
		{
			windowQueue.Clear();
			windowHistory.Clear();

			foreach (WindowStateEntry current in state.WindowQueue)
			{
				WindowHistoryEntry entry = new WindowHistoryEntry(current.Window, current.Settings);
				current.Window.Hide(Transition.Null);
				current.Window.SetPayload(default);
				current.Window.SetViewModel(default);
				windowQueue.Enqueue(entry);
			}

			foreach (WindowStateEntry current in state.WindowHistory)
			{
				WindowHistoryEntry entry = new WindowHistoryEntry(current.Window, current.Settings);
				windowHistory.Push(entry);

				if (current.IsVisible)
				{
					current.Window.SetPayload(current.Settings.Payload);
					current.Window.SetViewModel(current.Settings.ViewModel);
					current.Window.Show(Transition.Null);
				}
				else
				{
					current.Window.Hide(Transition.Null);
					current.Window.SetPayload(default);
					current.Window.SetViewModel(default);
				}
			}

			if (windowHistory.Count > 0)
			{
				WindowHistoryEntry topmostWindow = windowHistory.Pop();

				if (windowHistory.Count > 0)
				{
					currentWindow = windowHistory.Peek().View;
				}

				ShowWindow(topmostWindow, false).RunAndForget();
			}
		}

		public override async Task HideAll()
		{
			SetCurrentWindow(null, true);

			Task[] tasks = new Task[registeredViews.Count];
			int i = 0;

			foreach (KeyValuePair<Type, IWindow> current in registeredViews)
			{
				tasks[i] = HideWindow(current.Value);
				i++;
			}

			await Task.WhenAll(tasks);

			priorityParaLayer.RefreshBackground();
			windowHistory.Clear();
		}

		public override void ReparentView(IView view, Transform viewTransform)
		{
			IWindow window = view as IWindow;
			bool doBaseReparent = true;

			if (window == null)
			{
				Debug.LogError($"View {viewTransform.name} is not a Window!");
			}
			else
			{
				if (window.IsPopup)
				{
					priorityParaLayer.AddView(viewTransform);
					doBaseReparent = false;
				}
			}

			if (doBaseReparent)
			{
				base.ReparentView(view, viewTransform);
			}
		}

		protected virtual void OnCurrentWindowChanged(IWindow oldWindow, IWindow newWindow, bool fromBack)
		{
			CurrentWindowChanged?.Invoke(oldWindow, newWindow, fromBack);
		}

		protected override void ProcessViewRegister(IWindow view)
		{
			base.ProcessViewRegister(view);

			view.CloseRequested += OnCloseRequestedByWindow;
			view.SetLayer(this);
		}

		protected override void ProcessViewUnregister(IWindow view)
		{
			base.ProcessViewUnregister(view);

			view.CloseRequested -= OnCloseRequestedByWindow;
			view.SetLayer(null);
		}

		protected override async Task ShowView(IWindow view, WindowShowSettings settings)
		{
			if (ShouldEnqueue(view, settings))
			{
				EnqueueWindow(view, settings);
			}
			else
			{
				await ShowInForeground(view, settings);
			}
		}

		protected override async Task HideView(IWindow view, WindowHideSettings settings)
		{
			if (view == CurrentWindow)
			{
				windowHistory.Pop();

				if (view.IsPopup && !NextWindowIsPopup())
				{
					priorityParaLayer.HideBackground();
				}

				IWindow windowToClose = view;
				IWindow windowToOpen = GetNextWindow();

				if (windowToClose == windowToOpen)
				{
					await HideWindow(windowToClose, settings?.Transition);
					await ShowNextWindow();
				}
				else
				{
					await Task.WhenAll(
						HideWindow(windowToClose, settings?.Transition),
						ShowNextWindow());
				}
			}
			else
			{
				Debug.LogErrorFormat
				(
					"Hide requested on Window {0} but that's not the currently open one ({1})! Ignoring request.",
					view.GetType().Name,
					CurrentWindow != null ? CurrentWindow.GetType().Name : "current is null"
				);
			}
		}

		private List<IWindow> GetCurrentPath()
		{
			if (currentPathIsDirty)
			{
				currentPath = windowHistory.ToArray().Reverse().Select(x => x.View).ToList();
				currentPathIsDirty = false;
			}

			return currentPath;
		}

		private void SetCurrentWindow(IWindow newWindow, bool fromBack)
		{
			IWindow oldWindow = currentWindow;
			currentWindow = newWindow;
			currentPathIsDirty = true;
			OnCurrentWindowChanged(oldWindow, currentWindow, fromBack);
		}

		private bool NextWindowIsPopup()
		{
			bool nextWindowInQueueIsPopup = windowQueue.Count > 0 && windowQueue.Peek().View.IsPopup;
			bool lastWindowInHistoryIsPopup = windowHistory.Count > 0 && windowHistory.Peek().View.IsPopup;

			return nextWindowInQueueIsPopup || (windowQueue.Count == 0 && lastWindowInHistoryIsPopup);
		}

		private IWindow GetNextWindow()
		{
			IWindow result = null;

			if (windowQueue.Count > 0)
			{
				result = windowQueue.Peek().View;
			}
			else if (windowHistory.Count > 0)
			{
				result = windowHistory.Peek().View;
			}

			return result;
		}

		private async Task HideWindow(IWindow window, ITransition overrideTransition = null)
		{
			await window.Hide(overrideTransition);
		}

		private async Task ShowNextWindow()
		{
			if (windowQueue.Count > 0)
			{
				await ShowNextInQueue();
			}
			else if (windowHistory.Count > 0)
			{
				await ShowPreviousInHistory();
			}
			else
			{
				SetCurrentWindow(null, true);
			}
		}

		private async Task ShowNextInQueue()
		{
			if (windowQueue.Count > 0)
			{
				WindowHistoryEntry entry = windowQueue.Dequeue();

				await ShowWindow(entry, true);
			}
		}

		private async Task ShowPreviousInHistory()
		{
			if (windowHistory.Count > 0)
			{
				WindowHistoryEntry window = windowHistory.Pop();

				await ShowWindow(window, true);
			}
		}

		private void OnCloseRequestedByWindow(IView controller)
		{
			if (uiFrame.CurrentWindow == controller)
			{
				uiFrame.CloseCurrentWindow().Execute();
			}
			else
			{
				Debug.LogError($"You're trying to close a different window ({controller.GetType().Name}) than the current one.");
			}
		}

		private void OnPopupsBackgroundClicked()
		{
			if (CurrentWindow != null && CurrentWindow.IsPopup && CurrentWindow.CloseOnShadowClick)
			{
				uiFrame.CloseCurrentWindow().Execute();
			}
		}

		private bool ShouldEnqueue(IWindow window, WindowShowSettings settings)
		{
			WindowPriority priority = settings?.Priority ?? window.WindowPriority;

			return priority != WindowPriority.ForceForeground
			       && (CurrentWindow != null || windowQueue.Count > 0);
		}

		private void EnqueueWindow(IWindow window, WindowShowSettings settings)
		{
			windowQueue.Enqueue(new WindowHistoryEntry(window, settings));
		}

		private async Task ShowInForeground(IWindow window, WindowShowSettings settings)
		{
			await ShowInForeground(new WindowHistoryEntry(window, settings));
		}

		private async Task ShowInForeground(WindowHistoryEntry windowEntry)
		{
			if (CurrentWindow == windowEntry.View)
			{
				Debug.LogWarning($"[WindowLayer] The requested ({CurrentWindow.GetType().Name}) is already open!" +
				                 " This will add a duplicate to the history and might cause inconsistent behaviour." +
				                 " It is recommended that if you need to open the same view multiple times" +
				                 " (eg: when implementing a warning message pop-up), it closes itself upon the player input" +
				                 " that triggers the continuation of the flow.");
			}

			if (CurrentWindow != windowEntry.View
			    && CurrentWindow != null
			    && CurrentWindow.HideOnForegroundLost
			    && !windowEntry.View.IsPopup)
			{
				HideWindow(CurrentWindow, windowEntry.Settings.HideTransition).RunAndForget();
			}

			await ShowWindow(windowEntry, false);
		}

		private async Task ShowWindow(WindowHistoryEntry windowEntry, bool fromBack, ITransition overrideTransition = null)
		{
			if (windowEntry.View.IsPopup)
			{
				priorityParaLayer.ShowBackground();
			}

			windowHistory.Push(windowEntry);
			windowEntry.View.SetPayload(windowEntry.Settings.Payload);
			windowEntry.View.SetViewModel(ResolveViewModel(windowEntry));
			SetCurrentWindow(windowEntry.View, fromBack);
			
			ITransition transition = SelectTransition(windowEntry, fromBack, overrideTransition);

			await windowEntry.View.Show(windowEntry.Settings.ShowTransition);
		}
		
		private ITransition SelectTransition(WindowHistoryEntry windowEntry, bool fromBack, ITransition overrideTransition)
		{
			ITransition transition = null;

			if (overrideTransition != null)
			{
				transition = overrideTransition;
			}
			else if (fromBack == false)
			{
				transition = windowEntry.Settings.DestinationShowTransition;
			}

			return transition;
		}
		
		private IViewModel ResolveViewModel(WindowHistoryEntry windowEntry)
		{
			IViewModel result = windowEntry.Settings.ViewModel;

			if (windowEntry.Settings.ViewModel == null)
			{
				result = windowEntry.View.GetNewViewModel();
				windowEntry.Settings.ViewModel = result;
			}

			return result;
		}
	}
}
