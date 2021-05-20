using System.Collections.Generic;
using Juice.Utils;
using UnityEngine;

namespace Juice.Pooling
{
	internal class PoolData
	{
		public ObjectPool Pool { get; }
		public GameObject Original { get; }
		public int InitialPoolSize { get; }
		public float GrowFactor { get; }
		public int CurrentSize { get; private set; }
		public HashSet<PoolItem> Items { get; }

		public PoolData(ObjectPool pool, GameObject original, int initialPoolSize, float growFactor)
		{
			Pool = pool;
			Original = original;
			InitialPoolSize = initialPoolSize;
			GrowFactor = growFactor;
			CurrentSize = 0;
			Items = new HashSet<PoolItem>();
			CreatePool();
		}

		public GameObject Spawn(Transform parent, bool worldPositionStays)
		{
			GameObject result = null;

			if (Items.Count <= 0)
			{
				GrowPool();
			}

			using (var enumerator = Items.GetEnumerator())
			{
				enumerator.MoveNext();
				PoolItem item = enumerator.Current;
				item.transform.SetParent(parent, worldPositionStays);
				Items.Remove(item);
				item.gameObject.SetActive(true);
				item.OnSpawn();
				result = item.gameObject;
			}

			return result;
		}

		public void Recycle(PoolItem item)
		{
			if (item.Pool)
			{
				if (item.Pool == Pool)
				{
					if (Items.Contains(item) == false)
					{
						Items.Add(item);
						item.OnRecycle();
						item.gameObject.SetActive(false);
						item.transform.SetParent(Pool.transform);
					}
				}
				else
				{
					item.Pool.Recycle(item.gameObject);
				}
			}
			else
			{
				Object.Destroy(item.gameObject);
			}
		}

		private void CreatePool()
		{
			for (int i = 0; i < InitialPoolSize; i++)
			{
				AddNewItem();
			}

			CurrentSize = InitialPoolSize;
		}

		private void GrowPool()
		{
			int newItemsCount = Mathf.Max(Mathf.RoundToInt(InitialPoolSize * GrowFactor), 1);

			for (int i = 0; i < newItemsCount; i++)
			{
				AddNewItem();
			}

			if (GrowFactor > 0)
			{
				CurrentSize += newItemsCount;
			}
		}

		private void AddNewItem()
		{
			GameObject newObject = Object.Instantiate(Original, Pool.transform);
			PoolItem newItem = newObject.GetOrAddComponent<PoolItem>();
			newItem.Original = Original;
			newItem.Pool = Pool;
			newObject.SetActive(false);
			Items.Add(newItem);
		}
	}
}
