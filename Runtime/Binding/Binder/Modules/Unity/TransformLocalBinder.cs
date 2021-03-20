using UnityEngine;

namespace Juice
{
	public class TransformLocalBinder : MonoBehaviour, IBinder<Vector3>
	{
		[SerializeField] private BindingInfo localPosition = new BindingInfo(typeof(IReadOnlyObservableVariable<Vector3>));
		[SerializeField] private BindingInfo localRotation = new BindingInfo(typeof(IReadOnlyObservableVariable<Quaternion>));
		[SerializeField] private BindingInfo localScale = new BindingInfo(typeof(IReadOnlyObservableVariable<Vector3>));

		private Transform transformCache;

		private VariableBinding<Vector3> positionBinding;
		private VariableBinding<Quaternion> rotationBinding;
		private VariableBinding<Vector3> scaleBinding;

		protected virtual void Awake()
		{
			transformCache = transform;

			positionBinding = new VariableBinding<Vector3>(localPosition, this);
			positionBinding.Property.Changed += OnPositionChanged;

			rotationBinding = new VariableBinding<Quaternion>(localRotation, this);
			rotationBinding.Property.Changed += OnRotationChanged;

			scaleBinding = new VariableBinding<Vector3>(localScale, this);
			scaleBinding.Property.Changed += OnScaleChanged;
		}

		protected virtual void OnEnable()
		{
			positionBinding.Bind();
			rotationBinding.Bind();
			scaleBinding.Bind();
		}

		protected virtual void OnDisable()
		{
			positionBinding.Unbind();
			rotationBinding.Unbind();
			scaleBinding.Unbind();
		}

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
