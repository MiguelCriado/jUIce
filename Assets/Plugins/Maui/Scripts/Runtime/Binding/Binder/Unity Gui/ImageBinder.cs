using UnityEngine;
using UnityEngine.UI;

namespace Maui
{
	[RequireComponent(typeof(Image))]
	public class ImageBinder : VariableBinder<Sprite>
	{
		private Image image;

		protected override void Awake()
		{
			base.Awake();

			image = GetComponent<Image>();
		}

		protected override void Refresh(Sprite value)
		{
			image.overrideSprite = value;
		}
	}
}