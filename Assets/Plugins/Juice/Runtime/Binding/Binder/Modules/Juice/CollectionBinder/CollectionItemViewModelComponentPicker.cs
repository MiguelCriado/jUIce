using System.Collections.Generic;
using Juice.Pooling;
using Juice.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace Juice
{
	public class CollectionItemViewModelComponentPicker : ItemPicker
	{
		[SerializeField] private List<CollectionItemViewModelComponent> prefabs;
		[SerializeField] private ObjectPool pool;

		private PrefabPicker<CollectionItemViewModelComponent> prefabPicker;
		private bool isInitialized;

		protected virtual void Awake()
		{
			EnsureInitialState();
		}

		public override GameObject SpawnItem(int index, object value, Transform parent)
		{
			EnsureInitialState();

			GameObject result;
			CollectionItemViewModelComponent bestPrefab = prefabPicker.FindBestPrefab(value);

			Assert.IsNotNull(bestPrefab, $"A suitable prefab could not be found for {value} ({value.GetType().GetPrettifiedName()}).");

			result = SpawnItem(bestPrefab, parent);

			return result;
		}

		public override GameObject ReplaceItem(int index, object oldValue, object newValue, GameObject currentItem, Transform parent)
		{
			EnsureInitialState();

			GameObject result = currentItem;

			var prefabForOldValue = prefabPicker.FindBestPrefab(oldValue);
			var prefabForNewValue = prefabPicker.FindBestPrefab(newValue);

			if (ReferenceEquals(prefabForOldValue, prefabForNewValue) == false)
			{
				result = SpawnItem(prefabForNewValue, parent).gameObject;
			}

			return result;
		}

		public override void DisposeItem(int index, GameObject item)
		{
			EnsureInitialState();

			if (pool)
			{
				pool.Recycle(item);
			}
			else
			{
				Destroy(item);
			}
		}

		private void EnsureInitialState()
		{
			if (isInitialized == false)
			{
				isInitialized = true;

				prefabPicker = new PrefabPicker<CollectionItemViewModelComponent>(prefabs);
			}
		}

		private GameObject SpawnItem(CollectionItemViewModelComponent bestPrefab, Transform parent)
		{
			GameObject result;

			if (pool)
			{
				result = pool.Spawn(bestPrefab, parent).gameObject;
			}
			else
			{
				result = Instantiate(bestPrefab, parent).gameObject;
			}

			return result;
		}
	}
}
