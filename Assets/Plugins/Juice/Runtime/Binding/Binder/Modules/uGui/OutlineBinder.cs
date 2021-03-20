using UnityEngine;
using UnityEngine.UI;

namespace Juice
{
	[RequireComponent(typeof(Outline))]
	public class OutlineBinder : MonoBehaviour, IBinder<Color>
	{
		[SerializeField] private BindingInfo effectColor = new BindingInfo(typeof(IReadOnlyObservableVariable<Color>));
		[SerializeField] private BindingInfo effectDistance = new BindingInfo(typeof(IReadOnlyObservableVariable<Vector2>));
		[SerializeField] private BindingInfo useGraphicAlpha = new BindingInfo(typeof(IReadOnlyObservableVariable<bool>));

		private Outline outline;
		private VariableBinding<Color> effectColorBinding;
		private VariableBinding<Vector2> effectDistanceBinding;
		private VariableBinding<bool> useGraphicAlphaBinding;

		protected virtual void Awake()
		{
			outline = GetComponent<Outline>();

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

		private void OnEffectColorChanged(Color newValue)
		{
			outline.effectColor = newValue;
		}

		private void OnEffectDistanceBindingChanged(Vector2 newValue)
		{
			outline.effectDistance = newValue;
		}

		private void OnUseGraphicAlphaChanged(bool newValue)
		{
			outline.useGraphicAlpha = newValue;
		}
	}
}
