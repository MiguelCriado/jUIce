using System.Collections.Generic;
using UnityEngine;

namespace Juice.Pooling
{
	public class ObjectPool : MonoBehaviour
	{
		public static ObjectPool Global => SingleObjectPool.Instance.GlobalPool;

		[SerializeField] private int initialPoolSize = 5 ;
		[SerializeField] private int maxPoolSize = 20;
		[SerializeField] private List<GameObject> poolPrefabs;

		private readonly Dictionary<GameObject, PoolData> cachedPools = new Dictionary<GameObject, PoolData>();
		private bool arePoolPrefabsCached;

		private void OnValidate()
		{
			initialPoolSize = Mathf.Max(initialPoolSize, 0);
			maxPoolSize = Mathf.Max(maxPoolSize, 1);
		}

		private void Awake()
		{
			EnsureCachedPools();
		}

		public void CreatePool(GameObject original, int initialSize)
		{
			if (cachedPools.ContainsKey(original) == false)
			{
				cachedPools.Add(original, new PoolData(this, original, initialSize, 2f));
			}
		}

		public void CreatePool<T>(T original, int initialSize) where T : Component
		{
			CreatePool(original.gameObject, initialSize);
		}

		public GameObject Spawn(GameObject original, Transform parent, bool worldPositionStays = true)
		{
			GameObject result = null;

			EnsureCachedPools();

			if (cachedPools.TryGetValue(original.gameObject, out PoolData pool))
			{
				result = pool.Spawn(parent, worldPositionStays);
			}

			return result;
		}

		public GameObject Spawn(GameObject original)
		{
			return Spawn(original, transform);
		}

		public T Spawn<T>(T original, Transform parent, bool worldPositionStays = true) where T : Component
		{
			return Spawn(original.gameObject, parent, worldPositionStays).GetComponent<T>();
		}

		public T Spawn<T>(T original) where T : Component
		{
			return Spawn(original.gameObject).GetComponent<T>();
		}

		public void Recycle(GameObject item)
		{
			PoolItem poolItem = item.GetComponent<PoolItem>();

			if (poolItem && cachedPools.TryGetValue(poolItem.Original, out PoolData pool))
			{
				pool.Recycle(poolItem);
			}
			else
			{
				Destroy(item);
			}
		}

		public void Recycle<T>(T item) where T : Component
		{
			Recycle(item.gameObject);
		}

		private void EnsureCachedPools()
		{
			if (poolPrefabs != null && arePoolPrefabsCached == false)
			{
				foreach (GameObject objectToPool in poolPrefabs)
				{
					CreatePool(objectToPool, initialPoolSize);
				}

				arePoolPrefabsCached = true;
			}
		}
	}
}
