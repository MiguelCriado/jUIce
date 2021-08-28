using Juice.Utils;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Juice
{
	[RequireComponent(typeof(Shadow))]
	public class ShadowBinder : ComponentBinder
	{
		[SerializeField] private BindingInfo effectColor = BindingInfo.Variable<Color>();
		[SerializeField] private BindingInfo effectDistance = BindingInfo.Variable<Vector2>();
		[SerializeField] private BindingInfo useGraphicAlpha = BindingInfo.Variable<bool>();

		private Shadow shadow;

		protected override void Awake()
		{
			base.Awake();

			shadow = GetComponent<Shadow>();

			RegisterVariable<Color>(effectColor).OnChanged(OnEffectColorChanged);
			RegisterVariable<Vector2>(effectDistance).OnChanged(OnEffectDistanceChanged);
			RegisterVariable<bool>(useGraphicAlpha).OnChanged(OnUseGraphicAlphaChanged);
		}

#if UNITY_EDITOR
		[MenuItem("CONTEXT/Shadow/Add Binder")]
		private static void AddBinder(MenuCommand command)
		{
			Shadow context = (Shadow) command.context;
			context.GetOrAddComponent<ShadowBinder>();
		}
#endif

		private void OnEffectColorChanged(Color newValue)
		{
			shadow.effectColor = newValue;
		}

		private void OnEffectDistanceChanged(Vector2 newValue)
		{
			shadow.effectDistance = newValue;
		}

		private void OnUseGraphicAlphaChanged(bool newValue)
		{
			shadow.useGraphicAlpha = newValue;
		}
	}
}
