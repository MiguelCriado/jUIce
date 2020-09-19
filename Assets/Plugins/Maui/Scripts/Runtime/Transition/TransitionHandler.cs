using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Maui
{
	public class TransitionHandler
	{
		public bool IsVisible { get; private set; }

		public async Task Show(RectTransform target, Transition transition)
		{
			await DoAnimation(target, transition,true);

			IsVisible = true;
		}

		public async Task Hide(RectTransform target, Transition transition)
		{
			await DoAnimation(target, transition, false);

			IsVisible = false;
			target.gameObject.SetActive(false);
		}

		private async Task DoAnimation(RectTransform target, Transition transition, bool isVisible)
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

				transition.PrepareForAnimation(target);

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