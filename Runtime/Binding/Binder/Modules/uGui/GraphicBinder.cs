using UnityEngine;
using UnityEngine.UI;

namespace Juice
{
	public class GraphicBinder : MonoBehaviour, IBinder<Color>
	{
		[SerializeField] private BindingInfo color = new BindingInfo(typeof(IReadOnlyObservableVariable<Color>));
		[SerializeField] private BindingInfo material = new BindingInfo(typeof(IReadOnlyObservableVariable<Material>));
		[SerializeField] private BindingInfo raycastTarget = new BindingInfo(typeof(IReadOnlyObservableVariable<bool>));

		private Graphic graphic;
		private VariableBinding<Color> colorBinding;
		private VariableBinding<Material> materialBinding;
		private VariableBinding<bool> raycastTargetBinding;

		protected virtual void Awake()
		{
			graphic = GetComponent<Graphic>();

			if (graphic)
			{
				colorBinding = new VariableBinding<Color>(color, this);
				colorBinding.Property.Changed += OnColorChanged;

				materialBinding = new VariableBinding<Material>(material, this);
				materialBinding.Property.Changed += OnMaterialChanged;

				raycastTargetBinding = new VariableBinding<bool>(raycastTarget, this);
				raycastTargetBinding.Property.Changed += OnRaycastTargetChanged;
			}
			else
			{
				Debug.LogError($"{nameof(GraphicBinder)} requires a {nameof(Graphic)} to work.", this);
			}
		}

		protected virtual void OnEnable()
		{
			colorBinding.Bind();
			materialBinding.Bind();
			raycastTargetBinding.Bind();
		}

		protected virtual void OnDisable()
		{
			colorBinding.Unbind();
			materialBinding.Unbind();
			raycastTargetBinding.Unbind();
		}

		private void OnColorChanged(Color newValue)
		{
			graphic.color = newValue;
		}

		private void OnMaterialChanged(Material newValue)
		{
			graphic.material = newValue;
		}

		private void OnRaycastTargetChanged(bool newValue)
		{
			graphic.raycastTarget = newValue;
		}
	}
}
