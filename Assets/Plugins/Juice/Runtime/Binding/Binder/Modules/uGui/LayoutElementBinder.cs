using Juice.Utils;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Juice
{
	[RequireComponent(typeof(LayoutElement))]
	public class LayoutElementBinder : ComponentBinder
	{
		[SerializeField] private BindingInfo ignoreLayout = BindingInfo.Variable<bool>();
		[SerializeField] private BindingInfo minWidth = BindingInfo.Variable<float>();
		[SerializeField] private BindingInfo minHeight = BindingInfo.Variable<float>();
		[SerializeField] private BindingInfo preferredWidth = BindingInfo.Variable<float>();
		[SerializeField] private BindingInfo preferredHeight = BindingInfo.Variable<float>();
		[SerializeField] private BindingInfo flexibleWidth = BindingInfo.Variable<float>();
		[SerializeField] private BindingInfo flexibleHeight = BindingInfo.Variable<float>();
		[SerializeField] private BindingInfo layoutPriority = BindingInfo.Variable<int>();

		private LayoutElement layoutElement;

		protected override void Awake()
		{
			base.Awake();

			layoutElement = GetComponent<LayoutElement>();

			RegisterVariable<bool>(ignoreLayout).OnChanged(OnIgnoreLayoutChanged);
			RegisterVariable<float>(minWidth).OnChanged(OnMinWidthChanged);
			RegisterVariable<float>(minHeight).OnChanged(OnMinHeightChanged);
			RegisterVariable<float>(preferredWidth).OnChanged(OnPreferredWidthChanged);
			RegisterVariable<float>(preferredHeight).OnChanged(OnPreferredHeightChanged);
			RegisterVariable<float>(flexibleWidth).OnChanged(OnFlexibleWidthChanged);
			RegisterVariable<float>(flexibleHeight).OnChanged(OnFlexibleHeightChanged);
			RegisterVariable<int>(layoutPriority).OnChanged(OnLayoutPriorityChanged);
		}

#if UNITY_EDITOR
		[MenuItem("CONTEXT/LayoutElement/Add Binder")]
		private static void AddBinder(MenuCommand command)
		{
			LayoutElement context = (LayoutElement) command.context;
			context.GetOrAddComponent<LayoutElementBinder>();
		}
#endif

		private void OnIgnoreLayoutChanged(bool newValue)
		{
			layoutElement.ignoreLayout = newValue;
		}

		private void OnMinWidthChanged(float newValue)
		{
			layoutElement.minWidth = newValue;
		}

		private void OnMinHeightChanged(float newValue)
		{
			layoutElement.minHeight = newValue;
		}

		private void OnPreferredWidthChanged(float newValue)
		{
			layoutElement.preferredWidth = newValue;
		}

		private void OnPreferredHeightChanged(float newValue)
		{
			layoutElement.preferredHeight = newValue;
		}

		private void OnFlexibleWidthChanged(float newValue)
		{
			layoutElement.flexibleWidth = newValue;
		}

		private void OnFlexibleHeightChanged(float newValue)
		{
			layoutElement.flexibleHeight = newValue;
		}

		private void OnLayoutPriorityChanged(int newValue)
		{
			layoutElement.layoutPriority = newValue;
		}
	}
}
