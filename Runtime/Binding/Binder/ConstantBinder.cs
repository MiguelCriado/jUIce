using UnityEngine;

namespace Juice
{
	public abstract class ConstantBinder<T> : MonoBehaviour, IBinder<T>
	{
		protected abstract ConstantBindingInfo<T> BindingInfo { get; }

		private VariableBinding<T> binding;

		protected virtual void Awake()
		{
			binding = new VariableBinding<T>(BindingInfo, this);
			binding.Property.Changed += OnBoundPropertyChanged;
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

		private void OnBoundPropertyChanged(T newValue)
		{
			Refresh(newValue);
		}
	}
}
