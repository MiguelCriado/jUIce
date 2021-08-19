using System.Collections.Generic;
using UnityEngine;

namespace Juice.Pooling
{
	public class PoolItem : MonoBehaviour, IPoolable
	{
		public bool IsActive { get; private set; }
		
		[HideInInspector] public ObjectPool Pool;
		[HideInInspector] public GameObject Original;

		private List<IPoolable> poolables;

		private void Awake()
		{
			EnsureInitialState();
		}

		public void Recycle(bool worldPositionStays = true)
		{
			Pool.Recycle(gameObject, worldPositionStays);
		}

		public void OnSpawn()
		{
			IsActive = true;
			EnsureInitialState();

			foreach (IPoolable poolable in poolables)
			{
				poolable.OnSpawn();
			}
		}

		public void OnRecycle()
		{
			IsActive = false;
			EnsureInitialState();

			foreach (IPoolable poolable in poolables)
			{
				poolable.OnRecycle();
			}
		}

		private void EnsureInitialState()
		{
			if (poolables == null)
			{
				poolables = new List<IPoolable>();
				IPoolable[] potentialPoolables = GetComponentsInChildren<IPoolable>(true);

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