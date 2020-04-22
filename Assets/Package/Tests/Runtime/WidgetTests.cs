using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Muui.Tests
{
	public class WidgetTests
	{
		private Widget widget;

		[SetUp]
		public void SetUp()
		{
			GameObject gameObject = new GameObject("Widget");
			widget = gameObject.AddComponent<Widget>();
		}

		[TearDown]
		public void TearDown()
		{
			Object.Destroy(widget.gameObject);
		}

		[UnityTest]
		[Timeout(1000)]
		public IEnumerable Show_WhenAnimatingWithoutTransition_ExitsShowState()
		{
			bool isAnimationFinished = false;

			async void DoShow()
			{
				await widget.Show();
				isAnimationFinished = true;
			}

			DoShow();

			yield return new WaitUntil(() => isAnimationFinished);
		}

		[UnityTest]
		[Timeout(1000)]
		public IEnumerable Hide_WhenAnimatingWithoutTransition_ExitsHideState()
		{
			bool isAnimationFinished = false;

			async void DoHide()
			{
				await widget.Show();
				isAnimationFinished = true;
			}

			DoHide();

			yield return new WaitUntil(() => isAnimationFinished);
		}
	}
}
