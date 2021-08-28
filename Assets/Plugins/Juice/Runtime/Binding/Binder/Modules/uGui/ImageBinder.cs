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
		[SerializeField] private BindingInfo sourceImage = BindingInfo.Variable<Sprite>();

		private Image imageComponent;

		protected override void Awake()
		{
			base.Awake();

			imageComponent = GetComponent<Image>();

			RegisterVariable<Sprite>(sourceImage).OnChanged(OnSourceImageChanged);
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
