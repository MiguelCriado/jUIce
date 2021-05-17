namespace Juice
{
	public delegate void ViewModelChangeEventHandler<T>(
		IViewModelProvider<T> source,
		T lastViewModel,
		T newViewModel)
		where T : IViewModel;

	public interface IViewModelProvider<T> where T : IViewModel
	{
		event ViewModelChangeEventHandler<T> ViewModelChanged;

		T ViewModel { get; }
	}
}
