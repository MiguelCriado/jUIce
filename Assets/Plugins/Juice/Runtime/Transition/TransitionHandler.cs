using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Juice
{
	public class TransitionHandler
	{
		public async Task Show(RectTransform target, ITransition transition)
		{
			await AnimateTransition(target, transition,true);
		}

		public async Task Hide(RectTransform target, ITransition transition)
		{
			await AnimateTransition(target, transition, false);

			target.gameObject.SetActive(false);
		}

		private async Task AnimateTransition(RectTransform target, ITransition transition, bool isVisible)
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

				try
				{
					transition.Prepare(target);

					await transition.Animate(target);

					transition.Cleanup(target);
				}
				catch (Exception e)
				{
					Debug.LogError(e);
				}
			}
		}
	}
}
