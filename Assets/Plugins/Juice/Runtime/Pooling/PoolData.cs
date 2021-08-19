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
		public Stack<PoolItem> Items { get; }

		private List<PoolItem> itemsToReparent;

		public PoolData(
			ObjectPool pool,
			GameObject original,
			int initialPoolSize,
			float growFactor,
			IEnumerable<GameObject> prewarmedItems)
		{
			itemsToReparent = new List<PoolItem>();
			
			Pool = pool;
			Original = original;
			InitialPoolSize = initialPoolSize;
			GrowFactor = growFactor;
			CurrentSize = 0;
			Items = new Stack<PoolItem>();
			CreatePool(prewarmedItems);
			
			LifecycleUtils.OnUpdate += Update;
			// TODO find a way to unsubscribe from the Update event
		}

		public GameObject Spawn(Transform parent, bool worldPositionStays)
		{
			GameObject result = null;

			if (Items.Count <= 0)
			{
				GrowPool();
			}

			PoolItem item = Items.Pop();

			if (item)
			{
				item.transform.SetParent(parent, worldPositionStays);
				item.gameObject.SetActive(true);
				item.OnSpawn();
				result = item.gameObject;
			}
			else
			{
				CurrentSize--;
				result = Spawn(parent, worldPositionStays);
			}
			
			result.transform.SetAsLastSibling();
			return result;
		}

		public void Recycle(PoolItem item, bool worldPositionStays)
		{
			if (item.Pool)
			{
				if (item.Pool == Pool)
				{
					if (Items.Contains(item) == false)
					{
						Items.Push(item);
						item.OnRecycle();
						item.gameObject.SetActive(false);
						itemsToReparent.Add(item);
					}
				}
				else
				{
					item.Pool.Recycle(item.gameObject, worldPositionStays);
				}
			}
			else
			{
				Object.Destroy(item.gameObject);
			}
		}

		public void Merge(int maxPoolSize, IEnumerable<GameObject> prewarmedItems)
		{
			int mergedMaxPoolSize = Mathf.Max(Mathf.CeilToInt(InitialPoolSize * GrowFactor), CurrentSize, maxPoolSize);

			var enumerator = prewarmedItems.GetEnumerator();

			while (CurrentSize < mergedMaxPoolSize && enumerator.MoveNext())
			{
				AddPrewarmedItem(enumerator.Current);
			}

			CurrentSize = mergedMaxPoolSize;
		}
		
		private void Update()
		{
			foreach (PoolItem current in itemsToReparent)
			{
				if (current && current.IsActive == false)
				{
					current.transform.SetParent(Pool.transform, false);
				}
			}
			
			itemsToReparent.Clear();
		}

		private void CreatePool(IEnumerable<GameObject> prewarmedItems)
		{
			if (prewarmedItems != null)
			{
				foreach (GameObject current in prewarmedItems)
				{
					AddPrewarmedItem(current);
				}
			}
			
			for (int i = Items.Count; i < InitialPoolSize; i++)
			{
				AddNewItem();
			}

			CurrentSize = Items.Count;
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

		private void AddPrewarmedItem(GameObject item)
		{
			PoolItem newItem = SetupItem(item);
			item.transform.SetParent(Pool.transform, false);
			Items.Push(newItem);
		}
		
		private void AddNewItem()
		{
			GameObject newObject = Object.Instantiate(Original, Pool.transform);
			PoolItem newItem = SetupItem(newObject);
			Items.Push(newItem);
		}

		private PoolItem SetupItem(GameObject source)
		{
			PoolItem result = source.GetOrAddComponent<PoolItem>();
			result.Original = Original;
			result.Pool = Pool;
			source.SetActive(false);
			return result;
		}
	}
}
