using UnityEngine;

namespace Juice
{
	public abstract class VariableBinder<T> : ComponentBinder
	{
		[SerializeField, Rename(nameof(BindingInfoName))]
		private BindingInfo bindingInfo = new BindingInfo(typeof(IReadOnlyObservableVariable<T>));

		protected virtual string BindingInfoName { get; } = nameof(bindingInfo);

		protected override void Awake()
		{
			base.Awake();

			RegisterVariable<T>(bindingInfo).OnChanged(OnBoundPropertyChanged);
		}

		protected abstract void Refresh(T value);

		private void OnBoundPropertyChanged(T newValue)
		{
			Refresh(newValue);
		}
	}
}
