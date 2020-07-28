using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Maui
{
	public class UIFrame : MonoBehaviour
	{
		private class ViewEntry
		{
			public IView ViewPrefab;
			public IView ViewInstance;
			public int ReferenceCount;

			public ViewEntry(IView viewPrefab, IView viewInstance)
			{
				ViewPrefab = viewPrefab;
				ViewInstance = viewInstance;
				ReferenceCount = 0;
			}
		}

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

		private Dictionary<Type, ViewEntry> registeredViews = new Dictionary<Type, ViewEntry>();

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

		public void RegisterView<T>(T viewPrefab) where T : IView
		{
			if (IsPrefabValid(viewPrefab))
			{
				Type viewType = viewPrefab.GetType();

				if (typeof(IPanel).IsAssignableFrom(viewType))
				{
					IPanel viewAsPanel = viewPrefab as IPanel;
					ProcessViewRegister(viewAsPanel, panelLayer);
				}
				else if (typeof(IWindow).IsAssignableFrom(viewType))
				{
					IWindow viewAsWindow = viewPrefab as IWindow;
					ProcessViewRegister(viewAsWindow, windowLayer);
				}
				else
				{
					Debug.LogError($"The View type {typeof(T).Name} must implement {typeof(IPanel).Name} or {typeof(IWindow).Name}.");
				}
			}
		}

		public void DisposeView<T>(T viewPrefab) where T : IView
		{
			Type viewType = viewPrefab.GetType();

			if (registeredViews.TryGetValue(viewType, out ViewEntry viewEntry)
				&& viewEntry.ViewPrefab == (IView) viewPrefab)
			{
				viewEntry.ReferenceCount--;

				if (viewEntry.ReferenceCount <= 0)
				{
					Component instanceAsComponent = viewEntry.ViewInstance as Component;

					if (instanceAsComponent != null)
					{
						Destroy(instanceAsComponent.gameObject);
					}

					registeredViews.Remove(viewType);
				}
			}
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
			Type viewType = typeof(T);

			return registeredViews.ContainsKey(viewType);
		}

		private bool IsPrefabValid(IView view)
		{
			Component viewAsComponent = view as Component;

			if (viewAsComponent == null)
			{
				Debug.LogError($"The View to register must derive from {typeof(Component).Name}");
				return false;
			}

			if (IsPrefab(viewAsComponent.gameObject) == false)
			{
				Debug.LogError($"{viewAsComponent.gameObject.name} must be a Prefab.");
				return false;
			}

			if (registeredViews.TryGetValue(view.GetType(), out ViewEntry entry) && entry.ViewPrefab != view)
			{
				Debug.LogError($"{view.GetType().Name} already registered with a different Prefab.");
				return false;
			}

			return true;
		}

		private bool IsPrefab(GameObject gameObject)
		{
			return gameObject.scene.rootCount == 0;
		}

		private void ProcessViewRegister<T>(T viewPrefab, BaseLayer<T> layer) where T : IView
		{
			Type viewType = viewPrefab.GetType();
			ViewEntry viewEntry;

			if (registeredViews.TryGetValue(viewType, out viewEntry) == false)
			{
				Component prefabAsComponent = viewPrefab as Component;
				Component viewInstance = Instantiate(prefabAsComponent, mainCanvas.transform);
				viewInstance.gameObject.SetActive(false);
				IView view = (IView) viewInstance;
				viewEntry = new ViewEntry(viewPrefab, view);
				layer.RegisterView((T)viewEntry.ViewInstance);
				layer.ReparentView(view, viewInstance.transform);
				registeredViews.Add(viewType, viewEntry);
			}

			viewEntry.ReferenceCount++;
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
