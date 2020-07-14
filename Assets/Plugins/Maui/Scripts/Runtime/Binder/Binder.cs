using UnityEngine;

namespace Maui
{
	public abstract class Binder<T> : MonoBehaviour, IBinder<T>
	{
		[SerializeField] private BindingInfo bindingInfo = new BindingInfo(typeof(T));

		private Binding<T> binding;

		protected virtual void Awake()
		{
			binding = new Binding<T>(bindingInfo, this);
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
