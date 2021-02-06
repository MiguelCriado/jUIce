using System.Collections.Generic;
using UnityEngine;

namespace Juice.Pooling
{
	public class PoolItem : MonoBehaviour, IPoolable
	{
		[HideInInspector] public ObjectPool Pool;
		[HideInInspector] public GameObject Original;

		private List<IPoolable> poolables;

		private void Awake()
		{
			poolables = new List<IPoolable>();
			IPoolable[] potentialPoolables = GetComponentsInChildren<IPoolable>();

			foreach (IPoolable next in potentialPoolables)
			{
				if (!ReferenceEquals(next, this))
				{
					poolables.Add(next);
				}
			}
		}

		public void Recycle()
		{
			Pool.Recycle(gameObject);
		}

		public void OnSpawn()
		{
			foreach (IPoolable poolable in poolables)
			{
				poolable.OnSpawn();
			}
		}

		public void OnRecycle()
		{
			foreach (IPoolable poolable in poolables)
			{
				poolable.OnRecycle();
			}
		}
	}
}
