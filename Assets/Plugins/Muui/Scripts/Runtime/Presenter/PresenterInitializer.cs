using System;
using UnityEngine;

namespace Muui
{
	public class PresenterInitializer : MonoBehaviour
	{
		[SerializeField] private UIFrame uiFrame;

		private void Reset()
		{
			uiFrame = FindObjectOfType<UIFrame>();
		}

		private void Awake()
		{
			if (uiFrame == null)
			{
				uiFrame = FindObjectOfType<UIFrame>();
			}
		}

		private void Start()
		{
			if (uiFrame)
			{
				IPresenter[] childPresenters = GetComponentsInChildren<IPresenter>(true);

				foreach (var presenter in childPresenters)
				{
					presenter.Initialize(uiFrame);
				}
			}
		}
	}
}
