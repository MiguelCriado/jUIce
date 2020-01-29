using System;
using UnityEngine;

namespace Muui
{
	public abstract class BaseTransition : MonoBehaviour
	{
		public abstract void Animate(Transform target, Action callbackWhenFinished);
	}
}
