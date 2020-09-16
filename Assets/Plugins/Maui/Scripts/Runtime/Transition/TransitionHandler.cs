using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Maui
{
	public class TransitionHandler
	{
		public bool IsVisible { get; private set; }

		public async Task Show(GameObject target, Transition transition)
		{
			await DoAnimation(target, transition,true);

			IsVisible = true;
		}

		public async Task Hide(GameObject target, Transition transition)
		{
			await DoAnimation(target, transition, false);

			IsVisible = false;
			target.SetActive(false);
		}

		private async Task DoAnimation(GameObject target, Transition transition, bool isVisible)
		{
			if (transition == null)
			{
				target.SetActive(isVisible);
			}
			else
			{
				if (isVisible && target.activeSelf == false)
				{
					target.SetActive(true);
				}

				transition.PrepareForAnimation(target.transform);

				try
				{
					await transition.Animate(target.transform);
				}
				catch (Exception e)
				{
					Debug.LogError(e);
				}
			}
		}
	}
}