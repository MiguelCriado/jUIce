using System.Threading.Tasks;
using UnityEngine;

namespace Juice
{
	public abstract class ComponentTransition : MonoBehaviour, ITransition
	{
		public virtual void Prepare(RectTransform target)
		{

		}

		public abstract Task Animate(RectTransform target);

		public virtual void Cleanup(RectTransform target)
		{

		}
	}
}
