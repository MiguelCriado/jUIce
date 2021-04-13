namespace Juice
{
	public interface IBindableViewModel<T> : IViewModel
	{
		void SetData(T value);
	}
}
