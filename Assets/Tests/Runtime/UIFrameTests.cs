using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TestTools;

namespace Muui.Tests
{
	public class UIFrameTests
	{
		private static readonly string PanelAPath = "Assets/Tests/Runtime/Prefabs/Panel A.prefab";
		private static readonly string WindowWithPropertiesPath = "Assets/Tests/Runtime/Prefabs/Window With Properties.prefab";

		private UIFrame uiFrame;

		[SetUp]
		public void Setup()
		{
			uiFrame = UIFrameUtility.CreateDefaultUIFrame();
		}

		[TearDown]
		public void TearDown()
		{
			Object.Destroy(uiFrame.gameObject);
			uiFrame = null;
		}

		[UnityTest]
		public IEnumerator _00_UIFrameObjectIsCreated()
		{
			Assert.NotNull(uiFrame);
			yield return null;
		}

		[UnityTest]
		public IEnumerator _01_MainCanvasIsPresent()
		{
			Assert.NotNull(uiFrame.MainCanvas);
			yield return null;
		}

		[UnityTest]
		public IEnumerator _02_CameraIsPresent()
		{
			Assert.NotNull(uiFrame.UICamera);
			yield return null;
		}

		[UnityTest]
		public IEnumerator _03_EventSystemIsPresent()
		{
			EventSystem eventSystem = uiFrame.GetComponentInChildren<EventSystem>();

			Assert.NotNull(eventSystem);
			yield return null;
		}

		[UnityTest]
		public IEnumerator _04_WindowLayerIsPresent()
		{
			WindowLayer windowLayer = uiFrame.GetComponentInChildren<WindowLayer>();

			Assert.NotNull(windowLayer);
			yield return null;
		}

		[UnityTest]
		public IEnumerator _05_PanelLayerIsPresent()
		{
			PanelLayer panelLayer = uiFrame.GetComponentInChildren<PanelLayer>();

			Assert.NotNull(panelLayer);
			yield return null;
		}

		[UnityTest]
		public IEnumerator _06_PanelIsRegistered()
		{
			uiFrame.Initialize();
			PanelA panelAPrefab = AssetDatabase.LoadAssetAtPath<PanelA>(PanelAPath);

			uiFrame.RegisterScreen(panelAPrefab);

			Assert.IsTrue(uiFrame.IsScreenRegistered<PanelA>());
			yield return null;
		}

		[UnityTest]
		public IEnumerator _07_PanelIsRegisteredThenDisposed()
		{
			uiFrame.Initialize();
			PanelA panelAPrefab = AssetDatabase.LoadAssetAtPath<PanelA>(PanelAPath);

			uiFrame.RegisterScreen(panelAPrefab);
			uiFrame.DisposeScreen(panelAPrefab);

			Assert.IsFalse(uiFrame.IsScreenRegistered<PanelA>());
			yield return null;
		}

		[UnityTest]
		public IEnumerator _08_PanelIsDestroyedOnDispose()
		{
			uiFrame.Initialize();
			PanelA panelAPrefab = AssetDatabase.LoadAssetAtPath<PanelA>(PanelAPath);

			uiFrame.RegisterScreen(panelAPrefab);
			PanelA panelInstance = uiFrame.GetComponentInChildren<PanelA>(true);
			GameObject panelObject = panelInstance.gameObject;
			uiFrame.DisposeScreen(panelAPrefab);

			yield return null;
			Assert.IsTrue(panelObject == null);
		}

		[UnityTest]
		public IEnumerator _09_PanelRegistrationKeptWhenMultipleReferencesAfterDispose()
		{
			uiFrame.Initialize();
			PanelA panelAPrefab = AssetDatabase.LoadAssetAtPath<PanelA>(PanelAPath);

			uiFrame.RegisterScreen(panelAPrefab);
			uiFrame.RegisterScreen(panelAPrefab);
			uiFrame.DisposeScreen(panelAPrefab);

			Assert.IsTrue(uiFrame.IsScreenRegistered<PanelA>());
			yield return null;
		}

		[UnityTest]
		public IEnumerator _10_PanelRegistrationLostWhenMultipleReferencesAfterMultipleDisposes()
		{
			uiFrame.Initialize();
			PanelA panelAPrefab = AssetDatabase.LoadAssetAtPath<PanelA>(PanelAPath);

			uiFrame.RegisterScreen(panelAPrefab);
			uiFrame.RegisterScreen(panelAPrefab);
			uiFrame.DisposeScreen(panelAPrefab);
			uiFrame.DisposeScreen(panelAPrefab);

			Assert.IsFalse(uiFrame.IsScreenRegistered<PanelA>());
			yield return null;
		}

		[UnityTest]
		public IEnumerator RegisterScreen_WhenWindowWithPropertiesRegistered_ScreenIsRegistered()
		{
			uiFrame.Initialize();
			WindowWithProperties windowPrefab = AssetDatabase.LoadAssetAtPath<WindowWithProperties>(WindowWithPropertiesPath);

			uiFrame.RegisterScreen(windowPrefab);

			Assert.IsTrue(uiFrame.IsScreenRegistered<WindowWithProperties>());
			yield return null;
		}

		[UnityTest]
		public IEnumerator ShowScreen_WhenPanelShown_PanelIsVisible()
		{
			bool isPanelShown = false;
			uiFrame.Initialize();
			PanelA panelAPrefab = AssetDatabase.LoadAssetAtPath<PanelA>(PanelAPath);
			uiFrame.RegisterScreen(panelAPrefab);
			PanelA panelAInstance = uiFrame.GetComponentInChildren<PanelA>(true);

			async void ShowPanel()
			{
				await uiFrame.ShowScreen<PanelA>();
				isPanelShown = true;
			}

			ShowPanel();

			yield return new WaitUntil(() => isPanelShown);

			Assert.IsTrue(panelAInstance.IsVisible);
			yield return null;
		}

		[UnityTest]
		public IEnumerator ShowScreen_WhenPanelShown_PanelIsActive()
		{
			bool isPanelShown = false;
			uiFrame.Initialize();
			PanelA panelAPrefab = AssetDatabase.LoadAssetAtPath<PanelA>(PanelAPath);
			uiFrame.RegisterScreen(panelAPrefab);
			PanelA panelAInstance = uiFrame.GetComponentInChildren<PanelA>(true);

			async void ShowPanel()
			{
				await uiFrame.ShowScreen<PanelA>();
				isPanelShown = true;
			}

			ShowPanel();

			yield return new WaitUntil(() => isPanelShown);

			Assert.IsTrue(panelAInstance.gameObject.activeSelf);
			yield return null;
		}

		[UnityTest]
		public IEnumerator ShowScreen_WhenWindowWithPropertiesRegistered_ShowScreen()
		{
			uiFrame.Initialize();
			WindowWithProperties windowPrefab = AssetDatabase.LoadAssetAtPath<WindowWithProperties>(WindowWithPropertiesPath);
			uiFrame.RegisterScreen(windowPrefab);

			uiFrame.ShowScreen<WindowWithProperties>();

			Assert.IsTrue(uiFrame.CurrentWindow.GetType() == typeof(WindowWithProperties));
			yield return null;
		}
	}
}
