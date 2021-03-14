using UnityEngine;

namespace Juice
{
	public abstract class VariableBinder<T> : MonoBehaviour, IBinder<T>
	{
		[SerializeField, Rename(nameof(BindingInfoName))]
		private BindingInfo bindingInfo = new BindingInfo(typeof(IReadOnlyObservableVariable<T>));

		protected virtual string BindingInfoName { get; } = nameof(bindingInfo);

		private VariableBinding<T> binding;

		protected virtual void Reset()
		{
			bindingInfo = new BindingInfo(typeof(IReadOnlyObservableVariable<T>));
		}

		protected virtual void Awake()
		{
			binding = new VariableBinding<T>(bindingInfo, this);
			binding.Property.Changed += BoundPropertyChangedHandler;
		}

		protected virtual void OnEnable()
		{
			binding.Bind();
		}

		protected virtual void OnDisable()
		{
			binding.Unbind();
		}

		protected abstract void Refresh(T value);

		private void BoundPropertyChangedHandler(T newValue)
		{
			Refresh(newValue);
		}
	}
}
