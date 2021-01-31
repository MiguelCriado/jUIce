using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Juice
{
	public class UIFrame : MonoBehaviour
	{
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

		public event WindowLayer.WindowChangeHandler CurrentWindowChanged;

		public Camera UICamera => MainCanvas.worldCamera;
		public IWindow CurrentWindow => windowLayer.CurrentWindow;

		[SerializeField] private bool initializeOnAwake = true;

		private Canvas mainCanvas;
		private PanelLayer panelLayer;
		private WindowLayer windowLayer;
		private GraphicRaycaster graphicRaycaster;

		private readonly Dictionary<Type, IView> registeredViews = new Dictionary<Type, IView>();
		private readonly HashSet<Type> viewsInTransition = new HashSet<Type>();

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
					windowLayer.CurrentWindowChanged += CurrentWindowChanged;
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

		internal async Task ShowPanel(PanelShowSettings settings)
		{
			RegisterTransition(settings.ViewType);

			await panelLayer.ShowView(settings);

			UnregisterTransition(settings.ViewType);
		}

		internal async Task ShowWindow(WindowShowSettings settings)
		{
			RegisterTransition(settings.ViewType);

			await windowLayer.ShowView(settings);

			UnregisterTransition(settings.ViewType);
		}

		internal async Task HidePanel(PanelHideSettings settings)
		{
			RegisterTransition(settings.ViewType);

			await panelLayer.HideView(settings);

			UnregisterTransition(settings.ViewType);
		}

		internal async Task HideWindow(WindowHideSettings settings)
		{
			RegisterTransition(settings.ViewType);

			await windowLayer.HideView(settings);

			UnregisterTransition(settings.ViewType);
		}

		private void RegisterTransition(Type viewType)
		{
			viewsInTransition.Add(viewType);

			if (viewsInTransition.Count == 1)
			{
				BlockInteraction();
			}
		}

		private void UnregisterTransition(Type viewType)
		{
			viewsInTransition.Remove(viewType);

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
				current.Value.AllowsInteraction = false;
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
				current.Value.AllowsInteraction = true;
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
