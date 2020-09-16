using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Maui
{
	public class Widget : MonoBehaviour
	{
		public bool IsVisible { get; private set; }

		public Transition InTransition
		{
			get => inTransition;
			set => inTransition = value;
		}

		public Transition OutTransition
		{
			get => outTransition;
			set => outTransition = value;
		}

		[Header("Widget Animations")]
		[SerializeField] private Transition inTransition;
		[SerializeField] private Transition outTransition;

		public async Task Show()
		{
			await DoAnimation(InTransition,true);

			IsVisible = true;
		}

		public async Task Hide()
		{
			await DoAnimation(OutTransition, false);

			IsVisible = false;
			gameObject.SetActive(false);
		}

		private async Task DoAnimation(Transition targetTransition, bool isVisible)
		{
			if (targetTransition == null)
			{
				gameObject.SetActive(isVisible);
			}
			else
			{
				if (isVisible && gameObject.activeSelf == false)
				{
					gameObject.SetActive(true);
				}

				targetTransition.PrepareForAnimation(transform);

				try
				{
					await targetTransition.Animate(transform);
				}
				catch (Exception e)
				{
					Debug.LogError(e);
				}
			}
		}
	}
}
