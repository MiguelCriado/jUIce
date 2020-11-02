using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Maui
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

		[SerializeField] private bool initializeOnAwake;

		private Canvas mainCanvas;
		private PanelLayer panelLayer;
		private WindowLayer windowLayer;
		private GraphicRaycaster graphicRaycaster;

		private readonly Dictionary<Type, IView> registeredViews = new Dictionary<Type, IView>();

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
		
		public void ShowPanel<T>(IViewModel viewModel, PanelOptions overrideOptions = null) where T : IPanel
		{
			ShowPanelAsync<T>(viewModel, overrideOptions).RunAndForget();
		}
		
		public async Task ShowPanelAsync<T>(IViewModel viewModel, PanelOptions overrideOptions = null) where T : IPanel
		{
			await panelLayer.ShowView(typeof(T), viewModel, overrideOptions);
		}

		public void ShowWindow<T>(IViewModel viewModel, WindowOptions overrideOptions = null) where T : IWindow
		{
			ShowWindowAsync<T>(viewModel, overrideOptions).RunAndForget();
		}
		
		public async Task ShowWindowAsync<T>(IViewModel viewModel, WindowOptions overrideOptions = null) where T : IWindow
		{
			await windowLayer.ShowView(typeof(T), viewModel, overrideOptions);
		}

		public void HideView<T>() where T : IView
		{
			HideViewAsync<T>().RunAndForget();
		}
		
		public async Task HideViewAsync<T>() where T : IView
		{
			Type viewType = typeof(T);

			if (typeof(IPanel).IsAssignableFrom(viewType))
			{
				await panelLayer.HideView(viewType);
			}
			else if (typeof(IWindow).IsAssignableFrom(viewType))
			{
				await windowLayer.HideView(viewType);
			}
			else
			{
				Debug.LogError($"The View type {typeof(T).Name} must implement {nameof(IPanel)} or {nameof(IWindow)}.");
			}
		}

		public void CloseCurrentWindow()
		{
			CloseCurrentWindowAsync().RunAndForget();
		}
		
		public async Task CloseCurrentWindowAsync()
		{
			if (CurrentWindow != null)
			{
				await windowLayer.HideView(CurrentWindow);
			}
		}

		public bool IsViewRegistered<T>() where T : IView
		{
			return registeredViews.ContainsKey(typeof(T));
		}
		
		internal void BlockInteraction()
		{
			if (graphicRaycaster)
			{
				graphicRaycaster.enabled = false;
			}
		}

		internal void UnblockInteraction()
		{
			if (graphicRaycaster)
			{
				graphicRaycaster.enabled = true;
			}
		}
		
		private void OnWindowOpening(IWindow openedWindow, IWindow closedWindow, WindowOpenReason reason)
		{
			WindowOpening?.Invoke(openedWindow, closedWindow, reason);
		}
		
		private void OnWindowOpened(IWindow openedWindow, IWindow closedWindow, WindowOpenReason reason)
		{
			WindowOpened?.Invoke(openedWindow, closedWindow, reason);
		}

		private void OnWindowClosing(IWindow closedWindow, IWindow nextWindow, WindowHideReason reason)
		{
			WindowClosing?.Invoke(closedWindow, nextWindow, reason);
		}
		
		private void OnWindowClosed(IWindow closedWindow, IWindow nextWindow, WindowHideReason reason)
		{
			WindowClosed?.Invoke(closedWindow, nextWindow, reason);
		}
		
		private void OnPanelOpening(IPanel panel)
		{
			PanelOpening?.Invoke(panel);
		}
		
		private void OnPanelOpened(IPanel panel)
		{
			PanelOpened?.Invoke(panel);
		}
		
		private void OnPanelClosing(IPanel panel)
		{
			PanelClosing?.Invoke(panel);
		}
		
		private void OnPanelClosed(IPanel panel)
		{
			PanelClosed?.Invoke(panel);
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

		private void ProcessViewRegister<TView, TOptions>(TView view, Layer<TView, TOptions> layer)
			where TView : IView
			where TOptions : IViewOptions
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
