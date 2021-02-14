using System.Collections.Generic;
using System.Threading.Tasks;
using Juice.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Juice
{
	public class WindowParaLayer : MonoBehaviour
	{
		internal delegate void WindowParaLayerEventHandler();

		internal event WindowParaLayerEventHandler BackgroundClicked;

		[SerializeField] private Widget backgroundWidget;

		private readonly List<GameObject> containedViews = new List<GameObject>();
		private Button backgroundButton;
		private bool isHiding;

		private void Awake()
		{
			InitializeShadowButton();
		}

		internal void SetBackgroundWidget(Widget backgroundWidget)
		{
			this.backgroundWidget = backgroundWidget;
			InitializeShadowButton();
		}

		public void AddView(Transform viewTransform)
		{
			viewTransform.SetParent(transform, false);
			containedViews.Add(viewTransform.gameObject);
		}

		public void RefreshBackground()
		{
			if (IsBackgroundVisible() != ShouldBackgroundBeVisible())
			{
				ToggleBackground();
			}
		}

		public void ToggleBackground()
		{
			if (IsBackgroundVisible())
			{
				HideBackground();
			}
			else
			{
				ShowBackground();
			}
		}

		public void ShowBackground()
		{
			if (backgroundWidget)
			{
				backgroundWidget.transform.SetAsLastSibling();
				backgroundWidget.Show().RunAndForget();
			}
		}

		public void HideBackground()
		{
			HideBackgroundAsync().RunAndForget();
		}

		private async Task HideBackgroundAsync()
		{
			if (backgroundWidget)
			{
				isHiding = true;

				await backgroundWidget.Hide();

				isHiding = false;
			}
		}

		private bool IsBackgroundVisible()
		{
			return backgroundWidget.IsVisible && isHiding == false;
		}

		private bool ShouldBackgroundBeVisible()
		{
			bool result = false;

			if (backgroundWidget)
			{
				int i = 0;

				while (result == false && i < containedViews.Count)
				{
					if (containedViews[i] && containedViews[i].activeSelf)
					{
						result = true;
					}

					i++;
				}
			}

			return result;
		}

		private void InitializeShadowButton()
		{
			if (backgroundWidget)
			{
				backgroundButton = backgroundWidget.GetOrAddComponent<Button>();
				backgroundButton.transition = Selectable.Transition.None;
				backgroundButton.onClick.RemoveListener(OnBackgroundClicked);
				backgroundButton.onClick.AddListener(OnBackgroundClicked);
			}
		}

		private void OnBackgroundClicked()
		{
			BackgroundClicked?.Invoke();
		}
	}
}
