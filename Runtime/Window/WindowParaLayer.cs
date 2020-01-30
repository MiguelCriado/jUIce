using System.Collections.Generic;
using UnityEngine;

namespace Muui
{
	public class WindowParaLayer : MonoBehaviour
	{
		[SerializeField] private GameObject backgroundShadow = null;

		private List<GameObject> containedScreens = new List<GameObject>();

		public void AddScreen(Transform screenTransform)
		{
			screenTransform.SetParent(transform, false);
			containedScreens.Add(screenTransform.gameObject);
		}

		public void RefreshDarken()
		{
			bool activateBackground = false;
			int i = 0;

			while (activateBackground == false && i < containedScreens.Count)
			{
				if (containedScreens[i] != null && containedScreens[i].activeSelf)
				{
					activateBackground = true;
				}

				i++;
			}

			backgroundShadow.SetActive(activateBackground);
		}

		public void DarkenBackground()
		{
			backgroundShadow.SetActive(true);
			backgroundShadow.transform.SetAsLastSibling();
		}
	}
}
