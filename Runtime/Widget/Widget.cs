using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Muui
{
	public class Widget : MonoBehaviour
	{
		public bool IsVisible { get; private set; }

		public BaseTransition InTransition
		{
			get => inTransition;
			set => inTransition = value;
		}

		public BaseTransition OutTransition
		{
			get => outTransition;
			set => outTransition = value;
		}

		[Header("Widget Animations")]
		[SerializeField] private BaseTransition inTransition;
		[SerializeField] private BaseTransition outTransition;

		public async Task Show()
		{
			DoAnimation(InTransition, OnInTransitionFinished, true);

			while (IsVisible == false)
			{
				await Task.Yield();
			}
		}

		public async Task Hide()
		{
			DoAnimation(OutTransition, OnOutTransitionFinished, false);

			while (IsVisible)
			{
				await Task.Yield();
			}
		}

		private void DoAnimation(BaseTransition targetTransition, Action callbackWhenFinished, bool isVisible)
		{
			if (targetTransition == null)
			{
				gameObject.SetActive(isVisible);
				callbackWhenFinished?.Invoke();
			}
			else
			{
				if (isVisible && gameObject.activeSelf == false)
				{
					gameObject.SetActive(true);
				}

				targetTransition.Animate(transform, callbackWhenFinished);
			}
		}

		private void OnInTransitionFinished()
		{
			IsVisible = true;
		}

		private void OnOutTransitionFinished()
		{
			IsVisible = false;
			gameObject.SetActive(false);
		}
	}
}
