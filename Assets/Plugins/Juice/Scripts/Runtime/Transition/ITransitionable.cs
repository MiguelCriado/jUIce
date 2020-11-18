using System.Threading.Tasks;

namespace Juice
{
	public interface ITransitionable
	{
		bool IsVisible { get; }
		
		Task Show();
		Task Hide();
	}
}