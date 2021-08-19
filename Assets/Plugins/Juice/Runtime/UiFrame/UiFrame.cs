using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Juice.Utils;
using UnityEngine;

namespace Juice
{
	public class UiFrame : MonoBehaviour
	{
		public event WindowLayer.WindowChangeHandler CurrentWindowChanged;
		public event InteractionBlockingTracker.StateChangeEventHandler IsInteractableChanged;

		public Canvas MainCanvas
		{
			get
			{
				if (mainCanvas == null)
				{
					mainCanvas = GetComponentInChildren<Canvas>();
				}

				return mainCanvas;
			}
		}

		public Camera UICamera => MainCanvas.worldCamera;
		public IEnumerable<IWindow> CurrentWindowPath => windowLayer.CurrentPath;
		public IWindow CurrentWindow => windowLayer.CurrentWindow;
		public bool IsInteractable => mainBlockingTracker.IsInteractable;

		[SerializeField] private bool initializeOnAwake = true;

		private Canvas mainCanvas;
		private PanelLayer panelLayer;
		private WindowLayer windowLayer;
		private InteractionBlockingTracker mainBlockingTracker;

		private readonly Dictionary<Type, IView> registeredViews = new Dictionary<Type, IView>();
		private readonly HashSet<object> blockInteractionRequesters = new HashSet<object>();

		private void Reset()
		{
			initializeOnAwake = true;
		}

		private void Awake()
		{
			if (initializeOnAwake)
			{
				Initialize();
			}
		}

		public virtual void Initialize()
		{
			if (panelLayer == null)
			{
				panelLayer = GetComponentInChildren<PanelLayer>();

				if (panelLayer == null)
				{
					Debug.LogError("UI Frame lacks Panel Layer!");
				}
				else
				{
					panelLayer.Initialize(this);
				}
			}

			if (windowLayer == null)
			{
				windowLayer = GetComponentInChildren<WindowLayer>();

				if (windowLayer == null)
				{
					Debug.LogError("UI Frame lacks Window Layer!");
				}
				else
				{
					windowLayer.Initialize(this);
					windowLayer.CurrentWindowChanged += OnCurrentWindowChanged;
				}
			}

			GameObject blockingTrackerTarget = MainCanvas ? MainCanvas.gameObject : gameObject;
			mainBlockingTracker = blockingTrackerTarget.GetOrAddComponent<InteractionBlockingTracker>();
			mainBlockingTracker.IsInteractableChanged += OnIsInteractableChanged;
		}

		public UiFrameState GetCurrentState()
		{
			return new UiFrameState(windowLayer.GetCurrentState(), panelLayer.GetCurrentState());
		}

		public void SetState(UiFrameState state)
		{
			IEnumerable<IView> allViews = state.WindowLayerState.WindowHistory.Select(x => x.Window as IView)
				.Concat(state.WindowLayerState.WindowQueue.Select(x => x.Window as IView))
				.Concat(state.PanelLayerState.VisiblePanels.Select(x => x.Panel));
			bool isStateValid = allViews.All(x => registeredViews.TryGetValue(x.GetType(), out IView existing) && x == existing);

			if (isStateValid)
			{
				windowLayer.SetState(state.WindowLayerState);
				panelLayer.SetState(state.PanelLayerState);
			}
			else
			{
				Debug.LogError("All Views included in the state must be already registered.");
			}
		}

		public void RegisterView<T>(T view) where T : IView
		{
			if (IsViewValid(view))
			{
				Type viewType = view.GetType();

				if (typeof(IPanel).IsAssignableFrom(viewType))
				{
					IPanel viewAsPanel = view as IPanel;
					ProcessViewRegister(viewAsPanel, panelLayer);
				}
				else if (typeof(IWindow).IsAssignableFrom(viewType))
				{
					IWindow viewAsWindow = view as IWindow;
					ProcessViewRegister(viewAsWindow, windowLayer);
				}
				else
				{
					Debug.LogError($"The View type {typeof(T).Name} must implement {nameof(IPanel)} or {nameof(IWindow)}.");
				}
			}
		}

		public void UnregisterView<T>(T view) where T : IView
		{
			Type viewType = view.GetType();

			if (registeredViews.ContainsKey(viewType))
			{
				if (typeof(IPanel).IsAssignableFrom(viewType))
				{
					IPanel viewAsPanel = view as IPanel;
					ProcessViewUnregister(viewAsPanel, panelLayer);
				}
				else if (typeof(IWindow).IsAssignableFrom(viewType))
				{
					IWindow viewAsWindow = view as IWindow;
					ProcessViewUnregister(viewAsWindow, windowLayer);
				}
			}
			else
			{
				Debug.LogError($"Provided view {viewType.Name} was not registered.");
			}
		}

		public IPanelShowLauncher ShowPanel<T>() where T : IPanel
		{
			return ShowPanel(typeof(T));
		}

		public IPanelShowLauncher ShowPanel(Type type)
		{
			IPanelShowLauncher result = null;

			if (typeof(IPanel).IsAssignableFrom(type))
			{
				result = new PanelShowLauncher(type, ShowPanel);
			}
			else
			{
				Debug.LogError($"The requested type {type.Name} must implement {nameof(IPanel)}");
			}
			
			return result;
		}

		public IWindowShowLauncher ShowWindow<T>() where T : IWindow
		{
			return ShowWindow(typeof(T));
		}

		public IWindowShowLauncher ShowWindow(Type type)
		{
			IWindowShowLauncher result = null;

			if (typeof(IWindow).IsAssignableFrom(type))
			{
				result = new WindowShowLauncher(type, ShowWindow);
			}
			else
			{
				Debug.LogError($"The requested type {type.Name} must implement {nameof(IWindow)}");
			}
			
			return result;
		}

		public IPanelHideLauncher HidePanel<T>() where T : IPanel
		{
			return HidePanel(typeof(T));
		}

		public IPanelHideLauncher HidePanel(Type type)
		{
			IPanelHideLauncher result = null;

			if (typeof(IPanel).IsAssignableFrom(type))
			{
				result = new PanelHideLauncher(type, HidePanel);
			}
			else
			{
				Debug.LogError($"The requested type {type.Name} must implement {nameof(IPanel)}");
			}
			
			return result;
		}

		public IWindowHideLauncher HideWindow<T>() where T : IWindow
		{
			return new WindowHideLauncher(typeof(T), HideWindow);
		}

		public IWindowHideLauncher CloseCurrentWindow()
		{
			return new WindowHideLauncher(CurrentWindow.GetType(), HideWindow);
		}

		public async Task HideAll()
		{
			await Task.WhenAll(windowLayer.HideAll(), panelLayer.HideAll());
		}

		public bool IsViewRegistered<T>() where T : IView
		{
			return IsViewRegistered(typeof(T));
		}

		public bool IsViewRegistered(Type type)
		{
			return registeredViews.ContainsKey(type);
		}

		public bool IsViewRegistered<T>(T view) where T : IView
		{
			return registeredViews.TryGetValue(view.GetType(), out IView registeredView) && ReferenceEquals(view,  registeredView);
		}

		public void BlockInteraction(object requester)
		{
			blockInteractionRequesters.Add(requester);

			if (blockInteractionRequesters.Count == 1)
			{
				BlockInteraction();
			}
		}

		public void UnblockInteraction(object requester)
		{
			blockInteractionRequesters.Remove(requester);

			if (blockInteractionRequesters.Count <= 0)
			{
				UnblockInteraction();
			}
		}

		protected virtual void OnCurrentWindowChanged(IWindow oldWindow, IWindow newWindow, bool fromBack)
		{
			CurrentWindowChanged?.Invoke(oldWindow, newWindow, fromBack);
		}

		protected virtual void OnIsInteractableChanged(bool isInteractable)
		{
			IsInteractableChanged?.Invoke(isInteractable);
		}

		private bool IsViewValid(IView view)
		{
			Component viewAsComponent = view as Component;

			if (viewAsComponent == null)
			{
				Debug.LogError($"The View to register must derive from {nameof(Component)}");
				return false;
			}

			if (registeredViews.ContainsKey(view.GetType()))
			{
				Debug.LogError($"{view.GetType().Name} already registered.");
				return false;
			}

			return true;
		}

		private void ProcessViewRegister<TView, TShowSettings, THideSettings>(TView view, Layer<TView, TShowSettings, THideSettings> layer)
			where TView : IView
			where TShowSettings : IViewShowSettings
			where THideSettings : IViewHideSettings
		{
			view.Hide(Transition.Null);

			if (view is Component viewAsComponent)
			{
				viewAsComponent.gameObject.SetActive(false);
				layer.ReparentView(view, viewAsComponent.transform);
			}

			Type viewType = view.GetType();
			registeredViews.Add(viewType, view);
			layer.RegisterView(view);

			view.Showing += OnViewShowing;
			view.Shown += OnViewShown;
			view.Hiding += OnViewHiding;
			view.Hidden += OnViewHidden;
		}

		private void ProcessViewUnregister<TView, TShowSettings, THideSettings>(TView view, Layer<TView, TShowSettings, THideSettings> layer)
			where TView : IView
			where TShowSettings : IViewShowSettings
			where THideSettings : IViewHideSettings
		{
			Component viewAsComponent = view as Component;

			if (viewAsComponent != null)
			{
				viewAsComponent.gameObject.SetActive(false);
				viewAsComponent.transform.SetParent(null);
			}

			Type viewType = view.GetType();
			registeredViews.Remove(viewType);
			layer.UnregisterView(view);

			view.Showing -= OnViewShowing;
			view.Shown -= OnViewShown;
			view.Hiding -= OnViewHiding;
			view.Hidden -= OnViewHidden;
		}

		private void OnViewShowing(ITransitionable view)
		{
			BlockInteraction(view);
		}

		private void OnViewShown(ITransitionable view)
		{
			UnblockInteraction(view);
		}

		private void OnViewHiding(ITransitionable view)
		{
			BlockInteraction(view);
		}

		private void OnViewHidden(ITransitionable view)
		{
			UnblockInteraction(view);
		}

		private async Task ShowPanel(PanelShowSettings settings)
		{
			await panelLayer.ShowView(settings);
		}

		private async Task ShowWindow(WindowShowSettings settings)
		{
			await windowLayer.ShowView(settings);
		}

		private async Task HidePanel(PanelHideSettings settings)
		{
			await panelLayer.HideView(settings);
		}

		private async Task HideWindow(WindowHideSettings settings)
		{
			await windowLayer.HideView(settings);
		}

		private void BlockInteraction()
		{
			if (mainBlockingTracker)
			{
				mainBlockingTracker.IsInteractable = false;
			}

			foreach (var current in registeredViews)
			{
				current.Value.IsInteractable = false;
			}
		}

		private void UnblockInteraction()
		{
			if (mainBlockingTracker)
			{
				mainBlockingTracker.IsInteractable = true;
			}

			foreach (var current in registeredViews)
			{
				current.Value.IsInteractable = true;
			}
		}
	}
}
