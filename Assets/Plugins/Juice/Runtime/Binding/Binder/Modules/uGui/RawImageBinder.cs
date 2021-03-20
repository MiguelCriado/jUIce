using UnityEngine;
using UnityEngine.UI;

namespace Juice
{
	[RequireComponent(typeof(RawImage))]
	public class RawImageBinder : GraphicBinder
	{
		[SerializeField] private BindingInfo texture = new BindingInfo(typeof(IReadOnlyObservableVariable<Texture>));

		private RawImage rawImageComponent;
		private VariableBinding<Texture> textureBinding;

		protected override void Awake()
		{
			base.Awake();

			rawImageComponent = GetComponent<RawImage>();

			textureBinding = new VariableBinding<Texture>(texture, this);
			textureBinding.Property.Changed += OnTextureChanged;
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			textureBinding.Bind();
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			textureBinding.Unbind();
		}

		private void OnTextureChanged(Texture newValue)
		{
			rawImageComponent.texture = newValue;
		}
	}
}
