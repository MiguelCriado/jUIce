using Juice.Utils;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Juice
{
	[RequireComponent(typeof(Shadow))]
	public class ShadowBinder : MonoBehaviour, IBinder<Color>
	{
		[SerializeField] private BindingInfo effectColor = new BindingInfo(typeof(IReadOnlyObservableVariable<Color>));
		[SerializeField] private BindingInfo effectDistance = new BindingInfo(typeof(IReadOnlyObservableVariable<Vector2>));
		[SerializeField] private BindingInfo useGraphicAlpha = new BindingInfo(typeof(IReadOnlyObservableVariable<bool>));

		private Shadow shadow;
		private VariableBinding<Color> effectColorBinding;
		private VariableBinding<Vector2> effectDistanceBinding;
		private VariableBinding<bool> useGraphicAlphaBinding;

		protected virtual void Awake()
		{
			shadow = GetComponent<Shadow>();

			effectColorBinding = new VariableBinding<Color>(effectColor, this);
			effectColorBinding.Property.Changed += OnEffectColorChanged;

			effectDistanceBinding = new VariableBinding<Vector2>(effectDistance, this);
			effectDistanceBinding.Property.Changed += OnEffectDistanceBindingChanged;

			useGraphicAlphaBinding = new VariableBinding<bool>(useGraphicAlpha, this);
			useGraphicAlphaBinding.Property.Changed += OnUseGraphicAlphaChanged;
		}

		protected virtual void OnEnable()
		{
			effectColorBinding.Bind();
			effectColorBinding.Bind();
			useGraphicAlphaBinding.Bind();
		}

		protected virtual void OnDisable()
		{
			effectColorBinding.Unbind();
			effectColorBinding.Unbind();
			useGraphicAlphaBinding.Unbind();
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

		private void OnEffectDistanceBindingChanged(Vector2 newValue)
		{
			shadow.effectDistance = newValue;
		}

		private void OnUseGraphicAlphaChanged(bool newValue)
		{
			shadow.useGraphicAlpha = newValue;
		}
	}
}
