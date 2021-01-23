using UnityEngine;

namespace Juice
{
	public abstract class Panel : Panel<NullViewModel>
	{

	}

	public abstract class Panel<T> : View<T>, IPanel
		where T : IViewModel
	{
		public PanelPriority Priority => priority;

		[Header("Panel Properties")]
		[SerializeField] private PanelPriority priority;
	}
}
