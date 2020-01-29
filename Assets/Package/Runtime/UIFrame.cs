using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Muui
{
	public class UIFrame : MonoBehaviour
	{
		public Canvas MainCanvas
		{
			get
			{
				if (mainCanvas == null)
				{
					mainCanvas = GetComponent<Canvas>();
				}

				return mainCanvas;
			}
		}

		public Camera UICamera
		{
			get { return MainCanvas.worldCamera; }
		}

		private Canvas mainCanvas;
	}
}
