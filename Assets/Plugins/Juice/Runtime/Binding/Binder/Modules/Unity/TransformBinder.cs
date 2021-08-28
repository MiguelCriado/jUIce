using Juice.Utils;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Juice
{
	public class TransformBinder : ComponentBinder
	{
		[SerializeField] private BindingInfo position = BindingInfo.Variable<Vector3>();
		[SerializeField] private BindingInfo rotation = BindingInfo.Variable<Quaternion>();

		private Transform transformCache;

		protected override void Awake()
		{
			base.Awake();

			transformCache = transform;

			RegisterVariable<Vector3>(position).OnChanged(OnPositionChanged);
			RegisterVariable<Quaternion>(rotation).OnChanged(OnRotationChanged);
		}


#if UNITY_EDITOR
		[MenuItem("CONTEXT/Transform/Add Binder")]
		private static void AddBinder(MenuCommand command)
		{
			Transform context = (Transform) command.context;
			context.GetOrAddComponent<TransformBinder>();
		}
#endif

		private void OnPositionChanged(Vector3 newValue)
		{
			transformCache.position = newValue;
		}

		private void OnRotationChanged(Quaternion newValue)
		{
			transformCache.rotation = newValue;
		}
	}
}
