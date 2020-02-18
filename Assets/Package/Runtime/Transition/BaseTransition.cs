using System.Threading.Tasks;
using UnityEngine;

namespace Muui
{
	public abstract class BaseTransition : MonoBehaviour
	{
		public abstract void PrepareForAnimation(Transform target);
		public abstract Task Animate(Transform target);
	}
}
