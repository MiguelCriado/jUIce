using System.Threading.Tasks;
using UnityEngine;

namespace Maui
{
	public abstract class BasePanelController : BasePanelController<IViewModel>
	{

	}

	public abstract class BasePanelController<T> : BaseScreenController<T>, IPanelController
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
