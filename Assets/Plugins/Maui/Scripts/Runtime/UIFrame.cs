using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Maui
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
					panelLayer.Initialize();
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
					windowLayer.Initialize();
					windowLayer.RequestViewBlock += OnRequestViewBlock;
					windowLayer.RequestViewUnblock += OnRequestViewUnblock;
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

		public bool IsViewRegistered<T>() where T : IView
		{
			return registeredViews.ContainsKey(typeof(T));
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

		private void OnRequestViewBlock()
		{
			if (graphicRaycaster != null)
			{
				graphicRaycaster.enabled = false;
			}
		}

		private void OnRequestViewUnblock()
		{
			if (graphicRaycaster != null)
			{
				graphicRaycaster.enabled = true;
			}
		}
	}
}
