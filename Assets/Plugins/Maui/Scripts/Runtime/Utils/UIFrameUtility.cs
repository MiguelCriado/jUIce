using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Maui
{
	public static class UIFrameUtility
	{
		private static readonly string UIMaskName = "UI";

		public static UIFrame CreateDefaultUIFrame()
		{
			GameObject uiFrame = new GameObject("UI Frame");
			UIFrame result = uiFrame.AddComponent<UIFrame>();

			Camera camera = CreateCamera();
			camera.transform.SetParent(uiFrame.transform);

			EventSystem eventSystem = CreateEventSystem();
			eventSystem.transform.SetParent(uiFrame.transform);

			Canvas mainCanvas = CreateCanvas(camera);
			mainCanvas.transform.SetParent(uiFrame.transform);

			PanelLayer panelLayer = CreatePanelLayer();
			panelLayer.transform.SetParent(mainCanvas.transform, false);
			
			WindowLayer windowLayer = CreateWindowLayer();
			windowLayer.transform.SetParent(mainCanvas.transform, false);

			GameObject priorityPanelLayer = CreateUIObject("Priority Panel Layer");
			priorityPanelLayer.transform.SetParent(mainCanvas.transform, false);

			WindowParaLayer priorityWindowLayer = CreatePriorityWindowLayer();
			priorityWindowLayer.transform.SetParent(mainCanvas.transform, false);
			windowLayer.SetPriorityWindow(priorityWindowLayer);
			
			GameObject tutorialPanelLayer = CreateUIObject("Tutorial Panel Layer");
			tutorialPanelLayer.transform.SetParent(mainCanvas.transform, false);

			GameObject blockerPanelLayer = CreateUIObject("Blocker Panel Layer");
			blockerPanelLayer.transform.SetParent(mainCanvas.transform, false);

			panelLayer.PriorityLayers = new PanelPriorityLayerList(new List<PanelPriorityLayerListEntry>()
			{
				new PanelPriorityLayerListEntry(PanelPriority.Prioritary, priorityPanelLayer.transform),
				new PanelPriorityLayerListEntry(PanelPriority.Tutorial, tutorialPanelLayer.transform),
				new PanelPriorityLayerListEntry(PanelPriority.Blocker, blockerPanelLayer.transform)
			});

			return result;
		}

		private static Camera CreateCamera()
		{
			GameObject gameObject = new GameObject("UI Camera");
			Camera result = gameObject.AddComponent<Camera>();
			result.cullingMask = 1 << LayerMask.NameToLayer(UIMaskName);
			;
			return result;
		}

		private static EventSystem CreateEventSystem()
		{
			GameObject gameObject = new GameObject("EventSystem");
			EventSystem result = gameObject.AddComponent<EventSystem>();
			gameObject.AddComponent<StandaloneInputModule>();
			return result;
		}

		private static Canvas CreateCanvas(Camera worldCamera)
		{
			GameObject gameObject = new GameObject("Main Canvas");
			gameObject.layer = LayerMask.NameToLayer(UIMaskName);

			Canvas result = gameObject.AddComponent<Canvas>();
			result.renderMode = RenderMode.ScreenSpaceCamera;
			result.worldCamera = worldCamera;
			result.sortingOrder = 1;

			CanvasScaler canvasScaler = gameObject.AddComponent<CanvasScaler>();
			canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
			canvasScaler.matchWidthOrHeight = 1;
			canvasScaler.referenceResolution = new Vector2(1920, 1080);

			gameObject.AddComponent<GraphicRaycaster>();

			return result;
		}

		private static WindowLayer CreateWindowLayer()
		{
			return CreateUIObject<WindowLayer>("Window Layer");
		}

		private static PanelLayer CreatePanelLayer()
		{
			return CreateUIObject<PanelLayer>("Panel Layer");
		}

		private static WindowParaLayer CreatePriorityWindowLayer()
		{
			WindowParaLayer result = CreateUIObject<WindowParaLayer>("Priority Window Layer");
			Image background = CreateUIObject<Image>("Background Shadow");
			background.color = new Color(0, 0, 0, 0.7f);
			background.transform.SetParent(result.transform, false);
			background.gameObject.SetActive(false);
			result.SetBackgroundShadow(background.gameObject);
			return result;
		}

		private static GameObject CreateUIObject(string objectName)
		{
			GameObject gameObject = new GameObject(objectName);
			gameObject.layer = LayerMask.NameToLayer(UIMaskName);
			gameObject.AddComponent<RectTransform>();
			FitRectTransform(gameObject.transform as RectTransform);
			return gameObject;
		}

		private static T CreateUIObject<T>(string objectName) where T : Component
		{
			GameObject gameObject = CreateUIObject(objectName);
			return gameObject.AddComponent<T>();
		}

		private static void FitRectTransform(RectTransform rectTransform)
		{
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.sizeDelta = Vector2.zero;
		}
	}
}
