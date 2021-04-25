using Juice.Utils;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Juice
{
	public class TransformLocalBinder : ComponentBinder
	{
		[SerializeField] private BindingInfo localPosition = new BindingInfo(typeof(IReadOnlyObservableVariable<Vector3>));
		[SerializeField] private BindingInfo localRotation = new BindingInfo(typeof(IReadOnlyObservableVariable<Quaternion>));
		[SerializeField] private BindingInfo localScale = new BindingInfo(typeof(IReadOnlyObservableVariable<Vector3>));

		private Transform transformCache;

		protected override void Awake()
		{
			base.Awake();

			transformCache = transform;

			RegisterVariable<Vector3>(localPosition).OnChanged(OnPositionChanged);
			RegisterVariable<Quaternion>(localRotation).OnChanged(OnRotationChanged);
			RegisterVariable<Vector3>(localScale).OnChanged(OnScaleChanged);
		}

#if UNITY_EDITOR
		[MenuItem("CONTEXT/Transform/Add Local Binder")]
		private static void AddBinder(MenuCommand command)
		{
			Transform context = (Transform) command.context;
			context.GetOrAddComponent<TransformLocalBinder>();
		}
#endif

		private void OnPositionChanged(Vector3 newValue)
		{
			transformCache.localPosition = newValue;
		}

		private void OnRotationChanged(Quaternion newValue)
		{
			transformCache.localRotation = newValue;
		}

		private void OnScaleChanged(Vector3 newValue)
		{
			transformCache.localScale = newValue;
		}
	}
}
