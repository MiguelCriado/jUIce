using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Maui
{
	public class WindowParaLayer : MonoBehaviour
	{
		internal delegate void WindowParaLayerEventHandler();

		internal event WindowParaLayerEventHandler ShadowClicked;

		[SerializeField] private GameObject backgroundShadow = null;
		[SerializeField] private float shadowFadeTime;

		private List<GameObject> containedViews = new List<GameObject>();
		private CanvasGroup shadowCanvasGroup;
		private Button shadowButton;
		private bool isHiding;

		private void Reset()
		{
			shadowFadeTime = 0.3f;
		}

		private void Awake()
		{
			InitializeCanvasGroup();
			InitializeShadowButton();
		}

		internal void SetBackgroundShadow(GameObject backgroundShadow)
		{
			this.backgroundShadow = backgroundShadow;
			InitializeCanvasGroup();
			InitializeShadowButton();
		}

		public void AddView(Transform viewTransform)
		{
			viewTransform.SetParent(transform, false);
			containedViews.Add(viewTransform.gameObject);
		}

		public void RefreshDarken()
		{
			if (IsShadowVisible() != ShouldShadowBeVisible())
			{
				ToggleBackgroundShadow();
			}
		}

		public void ToggleBackgroundShadow()
		{
			if (IsShadowVisible())
			{
				HideBackgroundShadow();
			}
			else
			{
				ShowBackgroundShadow();
			}
		}

		public void ShowBackgroundShadow()
		{
			backgroundShadow.SetActive(true);
			backgroundShadow.transform.SetAsLastSibling();
			StopAllCoroutines();
			isHiding = false;
			StartCoroutine(DoTweenShadow(1f, shadowFadeTime));
		}

		public void HideBackgroundShadow()
		{
			StopAllCoroutines();
			StartCoroutine(InternalHideShadow());
		}

		private bool IsShadowVisible()
		{
			return backgroundShadow.activeSelf && isHiding == false;
		}

		private bool ShouldShadowBeVisible()
		{
			bool result = false;
			int i = 0;

			while (result == false && i < containedViews.Count)
			{
				if (containedViews[i] != null && containedViews[i].activeSelf)
				{
					result = true;
				}

				i++;
			}

			return result;
		}

		private IEnumerator InternalHideShadow()
		{
			isHiding = true;

			yield return StartCoroutine(DoTweenShadow(0, shadowFadeTime));

			backgroundShadow.gameObject.SetActive(false);
			isHiding = false;
		}

		private IEnumerator DoTweenShadow(float target, float duration)
		{
			float originalAlpha = shadowCanvasGroup.alpha;
			float startTime = Time.time;
			float elapsedTime;

			yield return null;

			while ((elapsedTime = Time.time - startTime) <= duration)
			{
				if (Math.Abs(elapsedTime) < Mathf.Epsilon)
				{
					elapsedTime = 0.05f;
				}

				float durationFraction = elapsedTime / duration;
				float tweenValue = Mathf.Lerp(originalAlpha, target, durationFraction);
				shadowCanvasGroup.alpha = tweenValue;

				yield return null;
			}

			shadowCanvasGroup.alpha = target;
		}

		private void InitializeCanvasGroup()
		{
			if (backgroundShadow != null)
			{
				shadowCanvasGroup = backgroundShadow.GetComponent<CanvasGroup>();

				if (shadowCanvasGroup == null)
				{
					shadowCanvasGroup = backgroundShadow.AddComponent<CanvasGroup>();
				}

				shadowCanvasGroup.alpha = 0;
			}
		}

		private void InitializeShadowButton()
		{
			if (backgroundShadow != null)
			{
				shadowButton = backgroundShadow.GetComponent<Button>();

				if (shadowButton == null)
				{
					shadowButton = backgroundShadow.AddComponent<Button>();
					shadowButton.transition = Selectable.Transition.None;
				}

				shadowButton.onClick.RemoveListener(OnShadowClicked);
				shadowButton.onClick.AddListener(OnShadowClicked);
			}
		}

		private void OnShadowClicked()
		{
			ShadowClicked?.Invoke();
		}
	}
}
