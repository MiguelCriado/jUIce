using Juice.Utils;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Juice
{
	[RequireComponent(typeof(Text))]
	public class TextBinder : GraphicBinder
	{
		[SerializeField] private BindingInfo text = BindingInfo.Variable<object>();

		private Text textComponent;

		protected override void Awake()
		{
			base.Awake();

			textComponent = GetComponent<Text>();

			RegisterVariable<object>(text).OnChanged(OnTextChanged);
		}

#if UNITY_EDITOR
		[MenuItem("CONTEXT/Text/Add Binder")]
		private static void AddBinder(MenuCommand command)
		{
			Text context = (Text) command.context;
			context.GetOrAddComponent<TextBinder>();
		}
#endif

		private void OnTextChanged(object newValue)
		{
			textComponent.text = newValue != null ? newValue.ToString() : string.Empty;
		}
	}
}
