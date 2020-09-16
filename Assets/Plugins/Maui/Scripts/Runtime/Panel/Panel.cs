using System.Threading.Tasks;
using UnityEngine;

namespace Maui
{
	public abstract class Panel : Panel<IViewModel>
	{

	}

	public abstract class Panel<T> : View<T>, IPanel
		where T : IViewModel
	{
		public PanelPriority Priority => priority;

		[Header("Panel Properties")]
		[SerializeField] private PanelPriority priority;
		
		public Task Show(IViewModel viewModel)
		{
			return base.Show((T) viewModel);
		}
	}
}
