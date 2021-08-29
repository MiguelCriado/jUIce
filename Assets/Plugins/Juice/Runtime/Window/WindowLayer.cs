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
		public delegate void HistoryEntryDiscardHandler(WindowHistoryEntry discardedEntry);

		public event WindowChangeHandler CurrentWindowChanged;
		public event HistoryEntryDiscardHandler HistoryEntryDiscarded;

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

				if (topmostWindow.View.IsPopup)
				{
					priorityParaLayer.ShowBackground();
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

		protected virtual void OnHistoryEntryDiscarded(WindowHistoryEntry discardedEntry)
		{
			HistoryEntryDiscarded?.Invoke(discardedEntry);
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
				WindowHistoryEntry entry = windowHistory.Pop();
				
				if (entry.Settings.BackDestinationType != null || settings.DestinationViewType != null)
				{
					Type destinationType = settings.DestinationViewType ?? entry.Settings.BackDestinationType;

					if (destinationType != null)
					{
						await RemoveHistoryUntilView(destinationType);
					}
				}
				
				if (view.IsPopup && !NextWindowIsPopup())
				{
					priorityParaLayer.HideBackground();
				}

				WindowHistoryEntry nextWindowEntry = GetNextWindowEntry();
				
				UpdateWindowPayload(nextWindowEntry, settings.Payload);

				IWindow windowToClose = view;
				IWindow windowToOpen = nextWindowEntry.View;

				ITransition hideTransition = settings.HideTransition ?? windowToClose.GetHideTransition(new WindowTransitionData(windowToOpen));
				ITransition showTransition = settings.ShowTransition ?? windowToOpen?.GetHideTransition(new WindowTransitionData(windowToClose));
				
				if (windowToClose == windowToOpen)
				{
					await HideWindow(windowToClose, hideTransition);
					await ShowNextWindow(showTransition);
				}
				else
				{
					await Task.WhenAll(
						HideWindow(windowToClose, hideTransition),
						ShowNextWindow(showTransition));
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
		
		private async Task RemoveHistoryUntilView(Type viewType)
		{
			if (windowHistory.Count > 0 && IsViewInHistory(viewType))
			{
				List<WindowHistoryEntry> discardedEntries = new List<WindowHistoryEntry>();

				while (EntryHasViewType(windowHistory.Peek(), viewType) == false)
				{
					discardedEntries.Add(windowHistory.Pop());
				}

				await DiscardHistoryEntries(discardedEntries);
			}
		}

		private async Task DiscardHistoryEntries(List<WindowHistoryEntry> entries)
		{
			IEnumerable<WindowHistoryEntry> activeEntries = entries.Where(x => x.View.IsVisible);

			await Task.WhenAll(activeEntries.Select(async x => await x.View.Hide()));

			entries.ForEach(OnHistoryEntryDiscarded);
		}

		private bool IsViewInHistory(Type viewType)
		{
			return windowHistory.Any(entry => EntryHasViewType(entry, viewType));
		}

		private static bool EntryHasViewType(WindowHistoryEntry entry, Type viewType)
		{
			return entry.Settings.StubViewType == viewType || entry.Settings.ViewType == viewType;
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

		private WindowHistoryEntry GetNextWindowEntry()
		{
			WindowHistoryEntry result = default;

			if (windowQueue.Count > 0)
			{
				result = windowQueue.Peek();
			}
			else if (windowHistory.Count > 0)
			{
				result = windowHistory.Peek();
			}

			return result;
		}
		
		private void UpdateWindowPayload(WindowHistoryEntry windowEntry, Dictionary<string, object> payload)
		{
			payload?.ToList().ForEach(pair => windowEntry.Settings.Payload[pair.Key] = pair.Value);
		}

		private async Task HideWindow(IWindow window, ITransition overrideTransition = null)
		{
			await window.Hide(overrideTransition);
		}

		private async Task ShowNextWindow(ITransition overrideTransition = null)
		{
			if (windowQueue.Count > 0)
			{
				await ShowNextInQueue(overrideTransition);
			}
			else if (windowHistory.Count > 0)
			{
				await ShowPreviousInHistory(overrideTransition);
			}
			else
			{
				SetCurrentWindow(null, true);
			}
		}

		private async Task ShowNextInQueue(ITransition overrideTransition = null)
		{
			if (windowQueue.Count > 0)
			{
				WindowHistoryEntry entry = windowQueue.Dequeue();

				await ShowWindow(entry, true, overrideTransition);
			}
		}

		private async Task ShowPreviousInHistory(ITransition overrideTransition = null)
		{
			if (windowHistory.Count > 0)
			{
				WindowHistoryEntry window = windowHistory.Pop();

				await ShowWindow(window, true, overrideTransition);
			}
		}

		private void OnCloseRequestedByWindow(IView controller)
		{
			uiFrame.CloseCurrentWindow().Execute();
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

			ITransition showTransition = null;
			
			if (IsGoingToDifferentWindow(windowEntry))
			{
				if (CurrentWindow.IsPopup)
				{
					await HideAllPopups();
					priorityParaLayer.HideBackground();
				}

				if (windowHistory.Count > 0)
				{
					WindowHistoryEntry lastOpenedWindow = windowHistory.Peek();

					showTransition = windowEntry.Settings.ShowTransition ?? windowEntry.View.GetShowTransition(new WindowTransitionData(lastOpenedWindow.View));
					ITransition hideTransition = windowEntry.Settings.HideTransition ?? lastOpenedWindow.View.GetHideTransition(new WindowTransitionData(windowEntry.View));

					if (lastOpenedWindow.View.HideOnForegroundLost)
					{
						HideWindow(lastOpenedWindow.View, hideTransition).RunAndForget();
					}	
				}
			}

			await ShowWindow(windowEntry, false, showTransition);
		}
		
		private bool IsGoingToDifferentWindow(WindowHistoryEntry windowEntry)
		{
			return CurrentWindow != null && CurrentWindow != windowEntry.View && !windowEntry.View.IsPopup;
		}

		private async Task HideAllPopups()
		{
			WindowHistoryEntry firstNonPopupEntry = GetFirstNonPopupEntry();

			using IEnumerator<WindowHistoryEntry> historyEnumerator = windowHistory.GetEnumerator();
			List<Task> hideTasks = new List<Task>();

			while (historyEnumerator.MoveNext() && !historyEnumerator.Current.Equals(firstNonPopupEntry))
			{
				hideTasks.Add(historyEnumerator.Current.View.Hide());
			}

			await Task.WhenAll(hideTasks);
		}

		private WindowHistoryEntry GetFirstNonPopupEntry()
		{
			return windowHistory.FirstOrDefault(x => !x.View.IsPopup);
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
			
			ITransition transition = overrideTransition ?? windowEntry.Settings.ShowTransition;

			await windowEntry.View.Show(transition);
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
