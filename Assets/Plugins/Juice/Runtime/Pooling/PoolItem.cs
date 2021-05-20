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
			EnsureInitialState();
		}

		public void Recycle()
		{
			Pool.Recycle(gameObject);
		}

		public void OnSpawn()
		{
			EnsureInitialState();

			foreach (IPoolable current in poolables)
			{
				current.OnSpawn();
			}
		}

		public void OnRecycle()
		{
			EnsureInitialState();

			foreach (IPoolable current in poolables)
			{
				current.OnRecycle();
			}
		}

		private void EnsureInitialState()
		{
			if (poolables == null)
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
		}
	}
}
