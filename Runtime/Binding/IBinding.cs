namespace Juice
{
	public interface IBinding
	{
		bool IsBound { get; }
		void Bind();
		void Unbind();
	}
}