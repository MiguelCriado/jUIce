using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Juice
{
	public class TransitionHandler
	{
		public bool IsVisible { get; private set; }

		public async Task Show(RectTransform target, ITransition transition)
		{
			if (IsVisible == false)
			{
				IsVisible = true;

				await DoAnimation(target, transition,true);
			}
		}

		public async Task Hide(RectTransform target, ITransition transition)
		{
			if (IsVisible)
			{
				IsVisible = false;

				await DoAnimation(target, transition, false);

				target.gameObject.SetActive(false);
			}
		}

		private async Task DoAnimation(RectTransform target, ITransition transition, bool isVisible)
		{
			if (transition == null)
			{
				target.gameObject.SetActive(isVisible);
			}
			else
			{
				if (isVisible && target.gameObject.activeSelf == false)
				{
					target.gameObject.SetActive(true);
				}

				transition.Prepare(target);

				try
				{
					await transition.Animate(target);
				}
				catch (Exception e)
				{
					Debug.LogError(e);
				}
			}
		}
	}
}
