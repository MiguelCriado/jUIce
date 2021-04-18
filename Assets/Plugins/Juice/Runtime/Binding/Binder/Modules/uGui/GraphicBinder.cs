using Juice.Utils;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Juice
{
	public class GraphicBinder : ComponentBinder
	{
		[SerializeField] private BindingInfo color = new BindingInfo(typeof(IReadOnlyObservableVariable<Color>));
		[SerializeField] private BindingInfo material = new BindingInfo(typeof(IReadOnlyObservableVariable<Material>));
		[SerializeField] private BindingInfo raycastTarget = new BindingInfo(typeof(IReadOnlyObservableVariable<bool>));

		private Graphic graphic;

		protected override void Awake()
		{
			base.Awake();

			graphic = GetComponent<Graphic>();

			if (graphic)
			{
				RegisterVariable<Color>(color).OnChanged(OnColorChanged);
				RegisterVariable<Material>(material).OnChanged(OnMaterialChanged);
				RegisterVariable<bool>(raycastTarget).OnChanged(OnRaycastTargetChanged);
			}
			else
			{
				Debug.LogError($"{nameof(GraphicBinder)} requires a {nameof(Graphic)} to work.", this);
			}
		}

#if UNITY_EDITOR
		[MenuItem("CONTEXT/Graphic/Add Binder")]
		private static void AddBinder(MenuCommand command)
		{
			Graphic context = (Graphic) command.context;
			context.GetOrAddComponent<GraphicBinder>();
		}
#endif

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
