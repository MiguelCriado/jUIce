using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Juice
{
	public enum WindowOpenReason
	{
		FocusGained,
		Opened
	}
		
	public enum WindowHideReason
	{
		FocusLost,
		Closed
	}
		
	public delegate void WindowOpenHandler(IWindow openedWindow, IWindow closedWindow, WindowOpenReason reason);
	public delegate void WindowCloseHandler(IWindow closedWindow, IWindow nextWindow, WindowHideReason reason);

	public class WindowLayer : Layer<IWindow, WindowOptions>
	{
		public event WindowOpenHandler WindowOpening;
		public event WindowOpenHandler WindowOpened;
		public event WindowCloseHandler WindowClosing;
		public event WindowCloseHandler WindowClosed;
		
		public IWindow CurrentWindow { get; private set; }

		[SerializeField] private WindowParaLayer priorityParaLayer = null;

		private readonly Queue<WindowHistoryEntry> windowQueue = new Queue<WindowHistoryEntry>();
		private readonly Stack<WindowHistoryEntry> windowHistory = new Stack<WindowHistoryEntry>();

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

		public override async Task HideView(IWindow view, WindowOptions overrideOptions = null)
		{
			if (view == CurrentWindow)
			{
				windowHistory.Pop();

				if (view.IsPopup && NextWindowIsPopup() == false)
				{
					priorityParaLayer.HideBackground();
				}

				IWindow windowToClose = view;
				IWindow windowToOpen = GetNextWindow();

				if (windowToClose == windowToOpen)
				{
					await HideAndNotify(windowToClose, windowToOpen, WindowHideReason.Closed, overrideOptions?.OutTransition);
					
					CurrentWindow = null;
					
					await ShowNextWindow(windowToClose);
				}
				else
				{
					CurrentWindow = null;
					
					await Task.WhenAll(
						HideAndNotify(windowToClose, windowToOpen, WindowHideReason.Closed),
						ShowNextWindow(windowToClose));
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

		public override async Task HideAll()
		{
			Task[] tasks = new Task[registeredViews.Count];
			int i = 0;

			foreach (KeyValuePair<Type, IWindow> current in registeredViews)
			{
				tasks[i] = HideAndNotify(current.Value, null, WindowHideReason.Closed);
				i++;
			}

			await Task.WhenAll(tasks);

			CurrentWindow = null;
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

		protected override void ProcessViewRegister(IWindow view)
		{
			base.ProcessViewRegister(view);

			view.CloseRequested += OnCloseRequestedByWindow;
		}

		protected override void ProcessViewUnregister(IWindow view)
		{
			base.ProcessViewUnregister(view);
			
			view.CloseRequested -= OnCloseRequestedByWindow;
		}
		
		protected override Task ShowView<TViewModel>(IWindow view, TViewModel viewModel, WindowOptions overrideOptions)
		{
			Task result;

			if (ShouldEnqueue(view, overrideOptions))
			{
				EnqueueWindow(view, viewModel, overrideOptions);
				result = Task.CompletedTask;
			}
			else
			{
				result = ShowInForeground(view, viewModel, overrideOptions, WindowOpenReason.Opened);
			}

			return result;
		}
		
		protected virtual void OnWindowClosing(IWindow windowToClose, IWindow windowToOpen, WindowHideReason reason)
		{
			WindowClosing?.Invoke(windowToClose, windowToOpen, reason);
		}
		
		protected virtual void OnWindowClosed(IWindow windowToClose, IWindow windowToOpen, WindowHideReason reason)
		{
			WindowClosed?.Invoke(windowToClose, windowToOpen, reason);
		}
		
		protected virtual void OnWindowOpening(IWindow windowToOpen, IWindow windowToClose, WindowOpenReason reason)
		{
			WindowOpening?.Invoke(windowToOpen, windowToClose, reason);
		}
		
		protected virtual void OnWindowOpened(IWindow windowToOpen, IWindow windowToClose, WindowOpenReason reason)
		{
			WindowOpened?.Invoke(windowToOpen, windowToClose, reason);
		}
		
		private async Task ShowNextWindow(IWindow closedWindow)
		{
			if (windowQueue.Count > 0)
			{
				await ShowNextInQueue(closedWindow);
			}
			else if (windowHistory.Count > 0)
			{
				await ShowPreviousInHistory(closedWindow);
			}
		}
		
		private async Task ShowNextInQueue(IWindow closedWindow)
		{
			if (windowQueue.Count > 0)
			{
				WindowHistoryEntry window = windowQueue.Dequeue();
				
				await ShowWindow(window, closedWindow, WindowOpenReason.Opened);
			}
		}

		private async Task ShowPreviousInHistory(IWindow closedWindow)
		{
			if (windowHistory.Count > 0)
			{
				WindowHistoryEntry window = windowHistory.Pop();
				
				await ShowWindow(window, closedWindow, WindowOpenReason.FocusGained);
			}
		}
		
		private async Task HideAndNotify(IWindow windowToClose, IWindow windowToOpen, WindowHideReason reason, Transition overrideTransition = null)
		{
			OnWindowClosing(windowToClose, windowToOpen, reason);

			await windowToClose.Hide(overrideTransition);
			
			OnWindowClosed(windowToClose, windowToOpen, reason);
		}

		private void OnCloseRequestedByWindow(IView controller)
		{
			HideView(controller as IWindow).RunAndForget();
		}

		private void OnPopupsBackgroundClicked()
		{
			if (CurrentWindow != null && CurrentWindow.IsPopup && CurrentWindow.CloseOnShadowClick)
			{
				HideView(CurrentWindow).RunAndForget();
			}
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

		private bool ShouldEnqueue(IWindow window, WindowOptions overrideOptions)
		{
			WindowPriority priority = overrideOptions?.Priority ?? window.WindowPriority;
			
			return priority != WindowPriority.ForceForeground 
			       && (CurrentWindow != null || windowQueue.Count > 0); 
		}

		private void EnqueueWindow(IWindow window, IViewModel viewModel, WindowOptions overrideOptions)
		{
			windowQueue.Enqueue(new WindowHistoryEntry(window, viewModel, overrideOptions));
		}
		
		private Task ShowInForeground(IWindow window, IViewModel viewModel, WindowOptions overrideOptions, WindowOpenReason reason)
		{
			return ShowInForeground(new WindowHistoryEntry(window, viewModel, overrideOptions), reason);
		}

		private async Task ShowInForeground(WindowHistoryEntry windowEntry, WindowOpenReason reason)
		{
			if (CurrentWindow == windowEntry.View)
			{
				Debug.LogWarning($"[WindowUILayer] The requested WindowId ({CurrentWindow.GetType().Name}) is already open!" +
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
				HideAndNotify(
					CurrentWindow,
					windowEntry.View,
					WindowHideReason.FocusLost,
					windowEntry.OverrideOptions?.OutTransition)
					.RunAndForget();
			}
			
			await ShowWindow(windowEntry, CurrentWindow, reason);
		}

		private async Task ShowWindow(WindowHistoryEntry windowEntry, IWindow closedWindow, WindowOpenReason reason)
		{
			if (windowEntry.View.IsPopup)
			{
				priorityParaLayer.ShowBackground();
			}

			windowHistory.Push(windowEntry);
			CurrentWindow = windowEntry.View;

			OnWindowOpening(windowEntry.View, closedWindow, reason);
			
			await windowEntry.Show();
			
			OnWindowOpened(windowEntry.View, closedWindow, reason);
		}

		private bool NextWindowIsPopup()
		{
			bool nextWindowInQueueIsPopup = windowQueue.Count > 0 && windowQueue.Peek().View.IsPopup;
			bool lastWindowInHistoryIsPopup = windowHistory.Count > 0 && windowHistory.Peek().View.IsPopup;

			return nextWindowInQueueIsPopup || (windowQueue.Count == 0 && lastWindowInHistoryIsPopup);
		}
	}
}
