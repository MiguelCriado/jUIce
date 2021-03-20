using Juice.Utils;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Juice
{
	[RequireComponent(typeof(Widget))]
	public class WidgetBinder : VariableBinder<bool>
	{
		protected override string BindingInfoName { get; } = "Show";

		private Widget widget;

		protected override void Awake()
		{
			base.Awake();

			widget = GetComponent<Widget>();
		}

		protected override void Refresh(bool value)
		{
			if (value)
			{
				widget.Show().RunAndForget();
			}
			else
			{
				widget.Hide().RunAndForget();
			}
		}

#if UNITY_EDITOR
		[MenuItem("CONTEXT/Widget/Add Binder")]
		private static void AddBinder(MenuCommand command)
		{
			Widget context = (Widget) command.context;
			context.GetOrAddComponent<WidgetBinder>();
		}
#endif
	}
}
