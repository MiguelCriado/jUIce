namespace Juice
{
	public abstract class ConstantBinder<T> : ComponentBinder
	{
		protected abstract ConstantBindingInfo<T> BindingInfo { get; }

		protected override void Awake()
		{
			base.Awake();

			RegisterVariable<T>(BindingInfo).OnChanged(OnBoundPropertyChanged);
		}

		protected abstract void Refresh(T value);

		private void OnBoundPropertyChanged(T newValue)
		{
			Refresh(newValue);
		}
	}
}
