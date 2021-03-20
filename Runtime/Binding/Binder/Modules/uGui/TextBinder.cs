using UnityEngine;
using UnityEngine.UI;

namespace Juice
{
	[RequireComponent(typeof(Text))]
	public class TextBinder : GraphicBinder
	{
		[SerializeField] private BindingInfo text = new BindingInfo(typeof(IReadOnlyObservableVariable<object>));

		private Text textComponent;
		private VariableBinding<object> textBinding;

		protected override void Awake()
		{
			base.Awake();

			textComponent = GetComponent<Text>();

			textBinding = new VariableBinding<object>(text, this);
			textBinding.Property.Changed += OnTextChanged;
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			textBinding.Bind();
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			textBinding.Unbind();
		}

		private void OnTextChanged(object newValue)
		{
			textComponent.text = newValue != null ? newValue.ToString() : string.Empty;

		}
	}
}
