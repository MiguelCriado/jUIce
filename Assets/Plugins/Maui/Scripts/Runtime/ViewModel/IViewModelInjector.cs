namespace Maui
{
	public interface IViewModelInjector<T> where T : IViewModel
	{
		ViewModelComponent Target { get; }
	}
}
