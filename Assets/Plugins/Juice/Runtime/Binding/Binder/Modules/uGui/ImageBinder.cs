using UnityEngine;
using UnityEngine.UI;

namespace Juice
{
	[RequireComponent(typeof(Image))]
	public class ImageBinder : GraphicBinder
	{
		[SerializeField] private BindingInfo sourceImage = new BindingInfo(typeof(IReadOnlyObservableVariable<Sprite>));

		private Image imageComponent;
		private VariableBinding<Sprite> sourceImageBinding;

		protected override void Awake()
		{
			base.Awake();

			imageComponent = GetComponent<Image>();

			sourceImageBinding = new VariableBinding<Sprite>(sourceImage, this);
			sourceImageBinding.Property.Changed += OnSourceImageChanged;
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			sourceImageBinding.Bind();
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			sourceImageBinding.Unbind();
		}

		private void OnSourceImageChanged(Sprite newValue)
		{
			imageComponent.overrideSprite = newValue;
		}
	}
}
