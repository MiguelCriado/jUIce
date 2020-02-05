using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TestTools;

namespace Muui.Tests
{
	public class UIFrameTests
	{
		private static readonly string UIFrameAddress = "UIFrameDefault";

		private UIFrame uiFrame;

		[SetUp]
		public void Setup()
		{
			uiFrame = MenuHelpers.CreateDefaultUIFrame();
		}

		[TearDown]
		public void TearDown()
		{
			Object.Destroy(uiFrame.gameObject);
			uiFrame = null;
		}

		[UnityTest]
		public IEnumerator _00UIFrameObjectIsCreated()
		{
			Assert.NotNull(uiFrame);
			yield return null;
		}

		[UnityTest]
		public IEnumerator _01UIFrameMainCanvasIsPresent()
		{
			Assert.NotNull(uiFrame.MainCanvas);
			yield return null;
		}

		[UnityTest]
		public IEnumerator _02UIFrameCameraIsPresent()
		{
			Assert.NotNull(uiFrame.UICamera);
			yield return null;
		}

		[UnityTest]
		public IEnumerator _03UIFrameEventSystemIsPresent()
		{
			EventSystem eventSystem = uiFrame.GetComponentInChildren<EventSystem>();
			Assert.NotNull(eventSystem);
			yield return null;
		}

		[UnityTest]
		public IEnumerator _04UIFrameWindowLayerIsPresent()
		{
			WindowLayer windowLayer = uiFrame.GetComponentInChildren<WindowLayer>();
			Assert.NotNull(windowLayer);
			yield return null;
		}

		[UnityTest]
		public IEnumerator _05UIFramePanelLayerIsPresent()
		{
			PanelLayer panelLayer = uiFrame.GetComponentInChildren<PanelLayer>();
			Assert.NotNull(panelLayer);
			yield return null;
		}
	}
}
