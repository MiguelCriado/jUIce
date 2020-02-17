using System.Threading.Tasks;
using UnityEngine;

namespace Muui
{
	public abstract class BaseTransition : MonoBehaviour
	{
		public abstract Task Animate(Transform target);
	}
}
