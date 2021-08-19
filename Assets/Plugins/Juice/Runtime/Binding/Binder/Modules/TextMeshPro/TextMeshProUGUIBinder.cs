using Juice.Utils;
using TMPro;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Juice
{
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class TextMeshProUGUIBinder : ComponentBinder
	{
		[SerializeField] private BindingInfo text = BindingInfo.Variable<object>();
		[SerializeField] private BindingInfo fontMaterial = BindingInfo.Variable<Material>();

		private TextMeshProUGUI textComponent;

		protected override void Awake()
		{
			base.Awake();

			textComponent = GetComponent<TextMeshProUGUI>();

			RegisterVariable<object>(text).OnChanged(OnTextChanged);
			RegisterVariable<Material>(fontMaterial).OnChanged(OnFontMaterialChanged);
		}

#if UNITY_EDITOR
		[MenuItem("CONTEXT/TextMeshProUGUI/Add Binder")]
		private static void AddBinder(MenuCommand command)
		{
			TextMeshProUGUI context = (TextMeshProUGUI) command.context;
			context.GetOrAddComponent<TextMeshProUGUIBinder>();
		}
#endif
		private void OnTextChanged(object value)
		{
			textComponent.text = value != null ? value.ToString() : string.Empty;
		}
		
		private void OnFontMaterialChanged(Material newValue)
		{
			textComponent.fontSharedMaterial = newValue;
		}
	}
}
