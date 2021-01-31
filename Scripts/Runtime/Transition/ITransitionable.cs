using System.Threading.Tasks;

namespace Juice
{
	public interface ITransitionable
	{
		bool IsVisible { get; }

		Task Show(Transition overrideTransition = null);
		Task Hide(Transition overrideTransition = null);
	}
}
