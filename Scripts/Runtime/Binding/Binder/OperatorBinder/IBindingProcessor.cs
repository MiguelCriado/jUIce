namespace Juice
{
	public interface IBindingProcessor
	{
		IViewModel ViewModel { get; }

		void Bind();
		void Unbind();
	}
}