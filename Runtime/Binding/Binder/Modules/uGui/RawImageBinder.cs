using Juice.Utils;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Juice
{
	[RequireComponent(typeof(RawImage))]
	public class RawImageBinder : GraphicBinder
	{
		[SerializeField] private BindingInfo texture = new BindingInfo(typeof(IReadOnlyObservableVariable<Texture>));

		private RawImage rawImageComponent;

		protected override void Awake()
		{
			base.Awake();

			rawImageComponent = GetComponent<RawImage>();

			RegisterVariable<Texture>(texture).OnChanged(OnTextureChanged);
		}

#if UNITY_EDITOR
		[MenuItem("CONTEXT/RawImage/Add Binder")]
		private static void AddBinder(MenuCommand command)
		{
			RawImage context = (RawImage) command.context;
			context.GetOrAddComponent<RawImageBinder>();
		}
#endif

		private void OnTextureChanged(Texture newValue)
		{
			rawImageComponent.texture = newValue;
		}
	}
}
