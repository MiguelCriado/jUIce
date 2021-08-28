using Juice.Utils;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Juice
{
	[RequireComponent(typeof(Outline))]
	public class OutlineBinder : ComponentBinder
	{
		[SerializeField] private BindingInfo effectColor = BindingInfo.Variable<Color>();
		[SerializeField] private BindingInfo effectDistance = BindingInfo.Variable<Vector2>();
		[SerializeField] private BindingInfo useGraphicAlpha = BindingInfo.Variable<bool>();

		private Outline outline;

		protected override void Awake()
		{
			base.Awake();

			outline = GetComponent<Outline>();

			RegisterVariable<Color>(effectColor).OnChanged(OnEffectColorChanged);
			RegisterVariable<Vector2>(effectDistance).OnChanged(OnEffectDistanceChanged);
			RegisterVariable<bool>(useGraphicAlpha).OnChanged(OnUseGraphicAlphaChanged);
		}

#if UNITY_EDITOR
		[MenuItem("CONTEXT/Outline/Add Binder")]
		private static void AddBinder(MenuCommand command)
		{
			Outline context = (Outline) command.context;
			context.GetOrAddComponent<OutlineBinder>();
		}
#endif

		private void OnEffectColorChanged(Color newValue)
		{
			outline.effectColor = newValue;
		}

		private void OnEffectDistanceChanged(Vector2 newValue)
		{
			outline.effectDistance = newValue;
		}

		private void OnUseGraphicAlphaChanged(bool newValue)
		{
			outline.useGraphicAlpha = newValue;
		}
	}
}
