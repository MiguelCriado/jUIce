using System.Collections.Generic;
using UnityEngine;

namespace Juice.Pooling
{
	public partial class ObjectPool : MonoBehaviour
	{
		[SerializeField] private int initialPoolSize = 5 ;
		[SerializeField] private int maxPoolSize = 20;
		[SerializeField] private List<GameObject> poolPrefabs;

		private Dictionary<GameObject, PoolData> cachedPools;

		private void OnValidate()
		{
			initialPoolSize = Mathf.Max(initialPoolSize, 0);
			maxPoolSize = Mathf.Max(maxPoolSize, 1);
		}

		private void Awake()
		{
			cachedPools = new Dictionary<GameObject, PoolData>();

			if (poolPrefabs != null)
			{
				foreach (GameObject objectToPool in poolPrefabs)
				{
					CreatePool(objectToPool, initialPoolSize);
				}
			}
		}

		public void CreatePool(GameObject original, int initialSize)
		{
			if (cachedPools.TryGetValue(original, out PoolData poolData) == false)
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
	}
}
