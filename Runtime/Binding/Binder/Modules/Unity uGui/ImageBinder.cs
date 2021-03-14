using UnityEngine;
using UnityEngine.UI;

namespace Juice
{
	[RequireComponent(typeof(Image))]
	public class ImageBinder : VariableBinder<Sprite>
	{
		protected override string BindingInfoName { get; } = "Source Image";

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
