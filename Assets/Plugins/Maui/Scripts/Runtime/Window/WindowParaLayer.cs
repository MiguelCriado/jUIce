using System.Collections.Generic;
using Maui.Tweening;
using Maui.Utils;
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

		private readonly List<GameObject> containedViews = new List<GameObject>();
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
			Tween.Kill(shadowCanvasGroup);
			isHiding = false;
			backgroundShadow.SetActive(true);
			backgroundShadow.transform.SetAsLastSibling();
			shadowCanvasGroup.Fade(1, shadowFadeTime);
		}

		public void HideBackgroundShadow()
		{
			Tween.Kill(shadowCanvasGroup);
			isHiding = true;
			shadowCanvasGroup.Fade(0, shadowFadeTime)
				.Completed += t =>
			{
				backgroundShadow.gameObject.SetActive(false);
				isHiding = false;
			};
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

		private void InitializeCanvasGroup()
		{
			if (backgroundShadow != null)
			{
				shadowCanvasGroup = backgroundShadow.GetOrAddComponent<CanvasGroup>();
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
