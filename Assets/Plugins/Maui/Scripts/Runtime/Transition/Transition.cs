using System.Threading.Tasks;
using UnityEngine;

namespace Maui
{
	public abstract class Transition : MonoBehaviour
	{
		public abstract void PrepareForAnimation(RectTransform target);

		public abstract Task Animate(RectTransform target);
	}
}
