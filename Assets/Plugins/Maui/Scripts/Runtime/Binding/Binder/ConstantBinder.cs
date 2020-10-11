using UnityEngine;

namespace Maui
{
	public abstract class ConstantBinder<T> : MonoBehaviour, IBinder<T>
	{
		protected abstract ConstantBindingInfo<T> BindingInfo { get; }
		
		private VariableBinding<T> binding;

		protected virtual void Awake()
		{
			binding = new VariableBinding<T>(BindingInfo, this);
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