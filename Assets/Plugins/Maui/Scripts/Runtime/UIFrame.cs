using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Maui
{
	public class UIFrame : MonoBehaviour
	{
		private class ScreenEntry
		{
			public IScreenController ScreenPrefab;
			public IScreenController ScreenInstance;
			public int ReferenceCount;

			public ScreenEntry(IScreenController screenPrefab, IScreenController screenInstance)
			{
				ScreenPrefab = screenPrefab;
				ScreenInstance = screenInstance;
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

		public IWindowController CurrentWindow => windowLayer.CurrentWindow;

		[SerializeField] private bool initializeOnAwake;

		private Canvas mainCanvas;
		private PanelLayer panelLayer;
		private WindowLayer windowLayer;
		private GraphicRaycaster graphicRaycaster;

		private Dictionary<Type, ScreenEntry> registeredScreens = new Dictionary<Type, ScreenEntry>();

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
					windowLayer.RequestScreenBlock += OnRequestScreenBlock;
					windowLayer.RequestScreenUnblock += OnRequestScreenUnblock;
				}
			}

			graphicRaycaster = MainCanvas.GetComponent<GraphicRaycaster>();
		}

		public void RegisterScreen<T>(T screenPrefab) where T : IScreenController
		{
			if (IsPrefabValid(screenPrefab))
			{
				Type screenType = typeof(T);

				if (typeof(IPanelController).IsAssignableFrom(screenType))
				{
					IPanelController screenAsPanel = screenPrefab as IPanelController;
					ProcessScreenRegister(screenAsPanel, panelLayer);
				}
				else if (typeof(IWindowController).IsAssignableFrom(screenType))
				{
					IWindowController screenAsWindow = screenPrefab as IWindowController;
					ProcessScreenRegister(screenAsWindow, windowLayer);
				}
				else
				{
					Debug.LogError($"The Screen type {typeof(T).Name} must implement {typeof(IPanelController).Name} or {typeof(IWindowController).Name}.");
				}
			}
		}

		public void DisposeScreen<T>(T screenPrefab) where T : IScreenController
		{
			Type screenType = screenPrefab.GetType();

			if (registeredScreens.TryGetValue(screenType, out ScreenEntry screenEntry)
				&& screenEntry.ScreenPrefab == (IScreenController) screenPrefab)
			{
				screenEntry.ReferenceCount--;

				if (screenEntry.ReferenceCount <= 0)
				{
					Component instanceAsComponent = screenEntry.ScreenInstance as Component;

					if (instanceAsComponent != null)
					{
						Destroy(instanceAsComponent.gameObject);
					}

					registeredScreens.Remove(screenType);
				}
			}
		}

		public async Task ShowScreen<T>() where T : IScreenController
		{
			Type screenType = typeof(T);

			if (typeof(IPanelController).IsAssignableFrom(screenType))
			{
				await panelLayer.ShowScreen(screenType);
			}
			else if (typeof(IWindowController).IsAssignableFrom(screenType))
			{
				await windowLayer.ShowScreen(screenType);
			}
			else
			{
				Debug.LogError($"The Screen type {typeof(T).Name} must implement {typeof(IPanelController).Name} or {typeof(IWindowController).Name}.");
			}
		}

		public async Task ShowScreen<T1, T2>(T2 properties) where T2 : IScreenProperties
		{
			Type screenType = typeof(T1);

			if (typeof(IPanelController).IsAssignableFrom(screenType))
			{
				await panelLayer.ShowScreen(screenType, properties);
			}
			else if (typeof(IWindowController).IsAssignableFrom(screenType))
			{
				await windowLayer.ShowScreen(screenType, properties);
			}
			else
			{
				Debug.LogError($"The Screen type {typeof(T1).Name} must implement {typeof(IPanelController).Name} or {typeof(IWindowController).Name}.");
			}
		}

		public async Task HideScreen<T>() where T : IScreenController
		{
			Type screenType = typeof(T);

			if (typeof(IPanelController).IsAssignableFrom(screenType))
			{
				await panelLayer.HideScreen(screenType);
			}
			else if (typeof(IWindowController).IsAssignableFrom(screenType))
			{
				await windowLayer.HideScreen(screenType);
			}
			else
			{
				Debug.LogError($"The Screen type {typeof(T).Name} must implement {typeof(IPanelController).Name} or {typeof(IWindowController).Name}.");
			}
		}

		public bool IsScreenRegistered<T>() where T : IScreenController
		{
			Type screenType = typeof(T);

			return registeredScreens.ContainsKey(screenType);
		}

		private bool IsPrefabValid(IScreenController screen)
		{
			Component screenAsComponent = screen as Component;

			if (screenAsComponent == null)
			{
				Debug.LogError($"The Screen to register must derive from {typeof(Component).Name}");
				return false;
			}

			if (IsPrefab(screenAsComponent.gameObject) == false)
			{
				Debug.LogError($"{screenAsComponent.gameObject.name} must be a Prefab.");
				return false;
			}

			if (registeredScreens.TryGetValue(screen.GetType(), out ScreenEntry entry) && entry.ScreenPrefab != screen)
			{
				Debug.LogError($"{screen.GetType().Name} already registered with a different Prefab.");
				return false;
			}

			return true;
		}

		private bool IsPrefab(GameObject gameObject)
		{
			return gameObject.scene.rootCount == 0;
		}

		private void ProcessScreenRegister<T>(T screenPrefab, BaseLayer<T> layer) where T : IScreenController
		{
			Type screenType = screenPrefab.GetType();
			ScreenEntry screenEntry;

			if (registeredScreens.TryGetValue(screenType, out screenEntry) == false)
			{
				Component prefabAsComponent = screenPrefab as Component;
				Component screenInstance = Instantiate(prefabAsComponent, mainCanvas.transform);
				screenInstance.gameObject.SetActive(false);
				IScreenController screenController = (IScreenController) screenInstance;
				screenEntry = new ScreenEntry(screenPrefab, screenController);
				layer.RegisterScreen((T)screenEntry.ScreenInstance);
				layer.ReparentScreen(screenController, screenInstance.transform);
				registeredScreens.Add(screenType, screenEntry);
			}

			screenEntry.ReferenceCount++;
		}

		private void OnRequestScreenBlock()
		{
			if (graphicRaycaster != null)
			{
				graphicRaycaster.enabled = false;
			}
		}

		private void OnRequestScreenUnblock()
		{
			if (graphicRaycaster != null)
			{
				graphicRaycaster.enabled = true;
			}
		}
	}
}
