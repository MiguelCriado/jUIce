using System.Collections.Generic;
using Juice.Pooling;
using UnityEngine;

namespace Juice
{
	public class AmountPrefabSpawner : ComponentBinder
	{
		[SerializeField] private BindingInfo amount = BindingInfo.Variable<int>();
		[SerializeField] private Transform container = default;
		[SerializeField] private GameObject prefab = default;
		[SerializeField] private ObjectPool pool = default;

		private Transform Container => container ? container : transform;

		private readonly Stack<GameObject> currentItems = new Stack<GameObject>();

		protected virtual void Reset()
		{
			container = transform;
			pool = GetComponent<ObjectPool>();
		}

		protected override void Awake()
		{
			base.Awake();

			if (pool && prefab)
			{
				pool.CreatePool(prefab, 3);
			}

			RegisterVariable<int>(amount).OnChanged(OnAmountChanged);
		}

		private void OnAmountChanged(int newValue)
		{
			while (currentItems.Count < newValue)
			{
				currentItems.Push(Spawn());
			}

			while (currentItems.Count > newValue)
			{
				Dispose(currentItems.Pop());
			}
		}

		private GameObject Spawn()
		{
			GameObject result;

			if (pool)
			{
				result = pool.Spawn(prefab, Container, false);
			}
			else
			{
				result = Instantiate(prefab, Container, false);
			}

			return result;
		}

		private void Dispose(GameObject item)
		{
			if (pool)
			{
				pool.Recycle(item);
			}
			else
			{
				Destroy(item);
			}
		}
	}
}