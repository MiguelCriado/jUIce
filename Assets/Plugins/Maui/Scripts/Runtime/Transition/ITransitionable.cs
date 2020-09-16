using System.Threading.Tasks;

namespace Maui
{
	public interface ITransitionable
	{
		bool IsVisible { get; }
		
		Task Show();
		Task Hide();
	}
}