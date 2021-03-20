using Juice.Utils;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

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

#if UNITY_EDITOR
		[MenuItem("CONTEXT/Image/Add Binder")]
		private static void AddBinder(MenuCommand command)
		{
			Image context = (Image) command.context;
			context.GetOrAddComponent<ImageBinder>();
		}
#endif

		private void OnSourceImageChanged(Sprite newValue)
		{
			imageComponent.overrideSprite = newValue;
		}
	}
}
