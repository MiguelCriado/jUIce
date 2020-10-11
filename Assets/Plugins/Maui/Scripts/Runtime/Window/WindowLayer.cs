using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Maui
{
	public class WindowLayer : Layer<IWindow>
	{
		public delegate void WindowOpenHandler(IWindow openedWindow, IWindow closedWindow);
		public delegate void WindowCloseHandler(IWindow closedWindow, IWindow nextWindow);

		public event WindowOpenHandler WindowOpening;
		public event WindowOpenHandler WindowOpened;
		public event WindowCloseHandler WindowClosing;
		public event WindowCloseHandler WindowClosed;

		public IWindow CurrentWindow { get; private set; }

		[SerializeField] private WindowParaLayer priorityParaLayer = null;

		private readonly Queue<WindowHistoryEntry> windowQueue = new Queue<WindowHistoryEntry>();
		private readonly Stack<WindowHistoryEntry> windowHistory = new Stack<WindowHistoryEntry>();
		private readonly HashSet<IView> viewsTransitioning = new HashSet<IView>();
		private bool IsViewTransitionInProgress => viewsTransitioning.Count > 0;

		protected virtual void OnEnable()
		{
			if (priorityParaLayer != null)
			{
				priorityParaLayer.ShadowClicked -= PopupsShadowClicked;
				priorityParaLayer.ShadowClicked += PopupsShadowClicked;
			}
		}

		internal void SetPriorityLayer(WindowParaLayer priorityParaLayer)
		{
			this.priorityParaLayer = priorityParaLayer;

			priorityParaLayer.ShadowClicked -= PopupsShadowClicked;
			priorityParaLayer.ShadowClicked += PopupsShadowClicked;
		}

		public override Task ShowView(IWindow view)
		{
			return ShowView<IViewModel>(view, null);
		}

		public override Task ShowView<TViewModel>(IWindow view, TViewModel viewModel)
		{
			Task result;

			if (ShouldEnqueue(view))
			{
				EnqueueWindow(view, viewModel);
				result = Task.CompletedTask;
			}
			else
			{
				result = DoShow(view, viewModel);
			}

			return result;
		}

		public override async Task HideView(IWindow view)
		{
			if (view == CurrentWindow)
			{
				windowHistory.Pop();
				AddTransition(view);

				if (view.IsPopup && NextWindowIsPopup() == false)
				{
					priorityParaLayer.HideBackgroundShadow();
				}

				IWindow windowToClose = view;
				IWindow windowToOpen = GetNextWindow();

				OnWindowClosing(windowToClose, windowToOpen);

				if (windowToClose == windowToOpen)
				{
					await view.Hide();
					
					CurrentWindow = null;
					
					await ShowNextWindow();
				}
				else
				{
					CurrentWindow = null;
					
					await Task.WhenAll(
						view.Hide(),
						ShowNextWindow());
				}

				OnWindowClosed(windowToClose, windowToOpen);
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

		public override async Task HideAll(bool animate = true)
		{
			await base.HideAll(animate);

			CurrentWindow = null;
			priorityParaLayer.RefreshDarken();
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

			view.InTransitionFinished += OnInAnimationFinished;
			view.OutTransitionFinished += OnOutAnimationFinished;
			view.CloseRequested += OnCloseRequestedByWindow;
		}

		protected override void ProcessViewUnregister(IWindow view)
		{
			base.ProcessViewUnregister(view);

			view.InTransitionFinished -= OnInAnimationFinished;
			view.OutTransitionFinished -= OnOutAnimationFinished;
			view.CloseRequested -= OnCloseRequestedByWindow;
		}
		
		protected virtual void OnWindowClosing(IWindow windowToClose, IWindow windowToOpen)
		{
			WindowClosing?.Invoke(windowToClose, windowToOpen);
		}
		
		protected virtual void OnWindowClosed(IWindow windowToClose, IWindow windowToOpen)
		{
			WindowClosed?.Invoke(windowToClose, windowToOpen);
		}
		
		protected virtual void OnWindowOpening(IWindow windowToOpen, IWindow windowToClose)
		{
			WindowOpening?.Invoke(windowToOpen, windowToClose);
		}
		
		protected virtual void OnWindowOpened(IWindow windowToOpen, IWindow windowToClose)
		{
			WindowOpened?.Invoke(windowToOpen, windowToClose);
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
		}

		private void OnInAnimationFinished(IView controller)
		{
			RemoveTransition(controller);
		}

		private void OnOutAnimationFinished(IView controller)
		{
			RemoveTransition(controller);

			if (controller is IWindow window && window.IsPopup)
			{
				priorityParaLayer.RefreshDarken();
			}
		}

		private void OnCloseRequestedByWindow(IView controller)
		{
			HideView(controller as IWindow).RunAndForget();
		}

		private void PopupsShadowClicked()
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

		private bool ShouldEnqueue(IWindow window)
		{
			bool CurrentWindowHasMorePriority() => CurrentWindow != null && CurrentWindow.IsPopup && window.IsPopup == false;
			bool NextWindowHasMorePriority() => windowQueue.Count > 0 && windowQueue.Peek().View.IsPopup && window.IsPopup == false;
			bool WantsToBeEnqueued() => window.WindowPriority != WindowPriority.ForceForeground;
			bool CurrentWindowHasSamePriority() => CurrentWindow != null && CurrentWindow.IsPopup == window.IsPopup;
			bool NextWindowHasSamePriority() => windowQueue.Count > 0 && windowQueue.Peek().View.IsPopup == window.IsPopup;
			return CurrentWindowHasMorePriority()
			       || NextWindowHasMorePriority()
			       || (WantsToBeEnqueued() && (CurrentWindowHasSamePriority() || NextWindowHasSamePriority()));
		}

		private void EnqueueWindow(IWindow window, IViewModel viewModel)
		{
			windowQueue.Enqueue(new WindowHistoryEntry(window, viewModel));
		}

		private async Task DoShow(WindowHistoryEntry windowEntry)
		{
			if (CurrentWindow == windowEntry.View)
			{
				Debug.LogWarning($"[WindowUILayer] The requested WindowId ({CurrentWindow.GetType().Name}) is already open!" +
				                 " This will add a duplicate to the history and might cause inconsistent behaviour." +
				                 " It is recommended that if you need to open the same view multiple times" +
				                 " (eg: when implementing a warning message pop-up), it closes itself upon the player input" +
				                 " that triggers the continuation of the flow.");
			}

			windowHistory.Push(windowEntry);
			AddTransition(windowEntry.View);

			if (windowEntry.View.IsPopup)
			{
				priorityParaLayer.ShowBackgroundShadow();
			}

			if (CurrentWindow != windowEntry.View
			    && CurrentWindow != null
			    && CurrentWindow.HideOnForegroundLost
			    && !windowEntry.View.IsPopup)
			{
				CurrentWindow.Hide().RunAndForget();
			}

			IWindow windowToOpen = windowEntry.View;
			IWindow windowToClose = CurrentWindow;

			OnWindowOpening(windowToOpen, windowToClose);
			
			CurrentWindow = windowEntry.View;

			await windowEntry.Show();
			
			OnWindowOpened(windowToOpen, windowToClose);
		}

		private Task DoShow(IWindow window, IViewModel viewModel)
		{
			return DoShow(new WindowHistoryEntry(window, viewModel));
		}

		private void AddTransition(IView view)
		{
			viewsTransitioning.Add(view);
			uiFrame.BlockInteraction();
		}

		private void RemoveTransition(IView view)
		{
			viewsTransitioning.Remove(view);

			if (IsViewTransitionInProgress == false)
			{
				uiFrame.UnblockInteraction();
			}
		}

		private bool NextWindowIsPopup()
		{
			bool nextWindowInQueueIsPopup = windowQueue.Count > 0 && windowQueue.Peek().View.IsPopup;
			bool lastWindowInHistoryIsPopup = windowHistory.Count > 0 && windowHistory.Peek().View.IsPopup;

			return nextWindowInQueueIsPopup || (windowQueue.Count == 0 && lastWindowInHistoryIsPopup);
		}

		private async Task ShowNextInQueue()
		{
			if (windowQueue.Count > 0)
			{
				WindowHistoryEntry window = windowQueue.Dequeue();
				await DoShow(window);
			}
		}

		private async Task ShowPreviousInHistory()
		{
			if (windowHistory.Count > 0)
			{
				WindowHistoryEntry window = windowHistory.Pop();
				await DoShow(window);
			}
		}
	}
}
