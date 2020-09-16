using System.Threading.Tasks;
using UnityEngine;

namespace Maui
{
	public abstract class Transition : MonoBehaviour
	{
		public abstract void PrepareForAnimation(Transform target);

		public abstract Task Animate(Transform target);
	}
}
