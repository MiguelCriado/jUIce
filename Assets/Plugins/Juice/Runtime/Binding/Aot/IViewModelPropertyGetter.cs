namespace Juice
{
	public interface IViewModelPropertyGetter
	{
		object GetProperty(IViewModel viewModel, string propertyName);
	}
}
