using System.Collections;
using NUnit.Framework;
using UnityEngine;
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
		public IEnumerator UIFrameObjectIsCreated()
		{
			Assert.True(uiFrame != null);
			yield return null;
		}
	}
}
