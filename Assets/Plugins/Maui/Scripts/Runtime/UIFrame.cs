using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Maui
{
	public class UIFrame : MonoBehaviour
	{
		public delegate void WindowOpenHandler(IWindow openedWindow, IWindow closedWindow);
		public delegate void WindowCloseHandler(IWindow closedWindow, IWindow nextWindow);

		public event WindowOpenHandler WindowOpening;
		public event WindowOpenHandler WindowOpened;
		public event WindowCloseHandler WindowClosing;
		public event WindowCloseHandler WindowClosed;
		
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

		private Dictionary<Type, IView> registeredViews = new Dictionary<Type, IView>();

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
					Debug.LogError($"The View type {typeof(T).Name} must implement {typeof(IPanel).Name} or {typeof(IWindow).Name}.");
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

		public async Task ShowView<T>() where T : IView
		{
			Type viewType = typeof(T);

			if (typeof(IPanel).IsAssignableFrom(viewType))
			{
				await panelLayer.ShowView(viewType);
			}
			else if (typeof(IWindow).IsAssignableFrom(viewType))
			{
				await windowLayer.ShowView(viewType);
			}
			else
			{
				Debug.LogError($"The View type {typeof(T).Name} must implement {typeof(IPanel).Name} or {typeof(IWindow).Name}.");
			}
		}

		public async Task ShowView<T>(IViewModel viewModel) where T : IView
		{
			Type viewType = typeof(T);

			if (typeof(IPanel).IsAssignableFrom(viewType))
			{
				await panelLayer.ShowView(viewType, viewModel);
			}
			else if (typeof(IWindow).IsAssignableFrom(viewType))
			{
				await windowLayer.ShowView(viewType, viewModel);
			}
			else
			{
				Debug.LogError($"The View type {typeof(T).Name} must implement {typeof(IPanel).Name} or {typeof(IWindow).Name}.");
			}
		}

		public async Task HideView<T>() where T : IView
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
				Debug.LogError($"The View type {typeof(T).Name} must implement {typeof(IPanel).Name} or {typeof(IWindow).Name}.");
			}
		}

		public async Task CloseCurrentWindow()
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
		
		private void OnWindowOpening(IWindow openedWindow, IWindow closedWindow)
		{
			WindowOpening?.Invoke(openedWindow, closedWindow);
		}
		
		private void OnWindowOpened(IWindow openedWindow, IWindow closedWindow)
		{
			WindowOpened?.Invoke(openedWindow, closedWindow);
		}

		private void OnWindowClosing(IWindow closedWindow, IWindow nextWindow)
		{
			WindowClosing?.Invoke(closedWindow, nextWindow);
		}
		
		private void OnWindowClosed(IWindow closedWindow, IWindow nextWindow)
		{
			WindowClosed?.Invoke(closedWindow, nextWindow);
		}

		private bool IsViewValid(IView view)
		{
			Component viewAsComponent = view as Component;

			if (viewAsComponent == null)
			{
				Debug.LogError($"The View to register must derive from {typeof(Component).Name}");
				return false;
			}

			if (registeredViews.ContainsKey(view.GetType()))
			{
				Debug.LogError($"{view.GetType().Name} already registered.");
				return false;
			}

			return true;
		}

		private void ProcessViewRegister<T>(T view, Layer<T> layer) where T : IView
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
