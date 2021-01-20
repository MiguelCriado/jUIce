using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Juice
{
	public class UIFrame : MonoBehaviour
	{
		public delegate void PanelOperationHandler(IPanel panel);

		public event WindowOpenHandler WindowOpening;
		public event WindowOpenHandler WindowOpened;
		public event WindowCloseHandler WindowClosing;
		public event WindowCloseHandler WindowClosed;
		public event PanelOperationHandler PanelOpening;
		public event PanelOperationHandler PanelOpened;
		public event PanelOperationHandler PanelClosing;
		public event PanelOperationHandler PanelClosed;

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

		public IWindow CurrentWindow => windowLayer.CurrentWindow;

		[SerializeField] private bool initializeOnAwake = true;

		private Canvas mainCanvas;
		private PanelLayer panelLayer;
		private WindowLayer windowLayer;
		private GraphicRaycaster graphicRaycaster;

		private readonly Dictionary<Type, IView> registeredViews = new Dictionary<Type, IView>();
		private readonly HashSet<IView> viewsInTransition = new HashSet<IView>();

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
					panelLayer.PanelOpening += OnPanelOpening;
					panelLayer.PanelOpened += OnPanelOpened;
					panelLayer.PanelClosing += OnPanelClosing;
					panelLayer.PanelClosed += OnPanelClosed;
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
					windowLayer.WindowOpening += OnWindowOpening;
					windowLayer.WindowOpened += OnWindowOpened;
					windowLayer.WindowClosing += OnWindowClosing;
					windowLayer.WindowClosed += OnWindowClosed;
					windowLayer.Initialize(this);
				}
			}

			graphicRaycaster = MainCanvas.GetComponent<GraphicRaycaster>();
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

		public void UnregisterView(Type viewType)
		{
			if (registeredViews.TryGetValue(viewType, out IView view))
			{
				Component viewAsComponent = view as Component;

				if (viewAsComponent != null)
				{
					viewAsComponent.gameObject.SetActive(false);
					viewAsComponent.transform.SetParent(null);
				}

				registeredViews.Remove(viewType);
			}
		}

		public void UnregisterView<T>() where T : IView
		{
			UnregisterView(typeof(T));
		}

		public IPanelShowLauncher ShowPanel<T>() where T : IPanel
		{
			return new PanelShowLauncher(this, typeof(T));
		}

		public IWindowShowLauncher ShowWindow<T>() where T : IWindow
		{
			return new WindowShowLauncher(this, typeof(T));
		}

		public IPanelHideLauncher HidePanel<T>() where T : IPanel
		{
			return new PanelHideLauncher(this, typeof(T));
		}

		public IWindowHideLauncher HideWindow<T>() where T : IWindow
		{
			return new WindowHideLauncher(this, typeof(T));
		}

		public IWindowHideLauncher CloseCurrentWindow()
		{
			return new WindowHideLauncher(this, CurrentWindow.GetType());
		}

		public bool IsViewRegistered<T>() where T : IView
		{
			return registeredViews.ContainsKey(typeof(T));
		}

		internal void ShowPanel(PanelShowSettings settings)
		{
			panelLayer.ShowView(settings);
		}

		internal void ShowWindow(WindowShowSettings settings)
		{
			windowLayer.ShowView(settings);
		}

		internal async Task HidePanel(PanelHideSettings settings)
		{
			await panelLayer.HideView(settings);
		}

		internal async Task HideWindow(WindowHideSettings settings)
		{
			await windowLayer.HideView(settings);
		}

		private void OnWindowOpening(IWindow openedWindow, IWindow closedWindow, WindowOpenReason reason)
		{
			OnViewStartsTransition(openedWindow);
			WindowOpening?.Invoke(openedWindow, closedWindow, reason);
		}

		private void OnWindowOpened(IWindow openedWindow, IWindow closedWindow, WindowOpenReason reason)
		{
			OnViewEndsTransition(openedWindow);
			WindowOpened?.Invoke(openedWindow, closedWindow, reason);
		}

		private void OnWindowClosing(IWindow closedWindow, IWindow nextWindow, WindowHideReason reason)
		{
			OnViewStartsTransition(closedWindow);
			WindowClosing?.Invoke(closedWindow, nextWindow, reason);
		}

		private void OnWindowClosed(IWindow closedWindow, IWindow nextWindow, WindowHideReason reason)
		{
			OnViewEndsTransition(closedWindow);
			WindowClosed?.Invoke(closedWindow, nextWindow, reason);
		}

		private void OnPanelOpening(IPanel panel)
		{
			OnViewStartsTransition(panel);
			PanelOpening?.Invoke(panel);
		}

		private void OnPanelOpened(IPanel panel)
		{
			OnViewEndsTransition(panel);
			PanelOpened?.Invoke(panel);
		}

		private void OnPanelClosing(IPanel panel)
		{
			OnViewStartsTransition(panel);
			PanelClosing?.Invoke(panel);
		}

		private void OnPanelClosed(IPanel panel)
		{
			OnViewEndsTransition(panel);
			PanelClosed?.Invoke(panel);
		}

		private void OnViewStartsTransition(IView view)
		{
			viewsInTransition.Add(view);

			if (viewsInTransition.Count == 1)
			{
				BlockInteraction();
			}
		}

		private void OnViewEndsTransition(IView view)
		{
			viewsInTransition.Remove(view);

			if (viewsInTransition.Count <= 0)
			{
				UnblockInteraction();
			}
		}

		private void BlockInteraction()
		{
			if (graphicRaycaster)
			{
				graphicRaycaster.enabled = false;
			}

			foreach (var current in registeredViews)
			{
				current.Value.AllowInteraction = false;
			}
		}

		private void UnblockInteraction()
		{
			if (graphicRaycaster)
			{
				graphicRaycaster.enabled = true;
			}

			foreach (var current in registeredViews)
			{
				current.Value.AllowInteraction = true;
			}
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
			Type viewType = view.GetType();
			Component viewAsComponent = view as Component;
			viewAsComponent.gameObject.SetActive(false);
			layer.RegisterView(view);
			layer.ReparentView(view, viewAsComponent.transform);
			registeredViews.Add(viewType, view);
		}
	}
}
