using System.Collections.Generic;
using Juice.Pooling;
using Juice.Utils;
using UnityEngine;

namespace Juice
{
	public class PrefabCollectionBinder : CollectionBinder<object>
	{
		public IReadOnlyList<CollectionItemViewModelComponent> CurrentItems => currentItems;

		[SerializeField] private List<CollectionItemViewModelComponent> prefabs;
		[SerializeField] private Transform itemsContainer;
		[SerializeField] private bool poolItems = true;

		protected override string BindingInfoName { get; } = "Collection";

		private Transform Container => itemsContainer ? itemsContainer : transform;

		private PrefabPicker<CollectionItemViewModelComponent> prefabPicker;
		private List<CollectionItemViewModelComponent> currentItems;
		private ObjectPool pool;

		protected virtual void OnValidate()
		{
			if (itemsContainer == null)
			{
				itemsContainer = transform;
			}
		}

		protected override void Awake()
		{
			base.Awake();

			prefabPicker = new PrefabPicker<CollectionItemViewModelComponent>(prefabs);
			currentItems = new List<CollectionItemViewModelComponent>();

			if (poolItems)
			{
				pool = this.GetOrAddComponent<ObjectPool>();

				foreach (CollectionItemViewModelComponent current in prefabs)
				{
					pool.CreatePool(current, 3);
				}
			}
		}

		protected override void OnCollectionReset()
		{
			ClearItems();
		}

		protected override void OnCollectionCountChanged(int oldCount, int newCount)
		{
			// Nothing to do here
		}

		protected override void OnCollectionItemAdded(int index, object value)
		{
			InsertItem(index, value);
		}

		protected override void OnCollectionItemMoved(int oldIndex, int newIndex, object value)
		{
			MoveItem(oldIndex, newIndex);
		}

		protected override void OnCollectionItemRemoved(int index, object value)
		{
			RemoveItem(index);
		}

		protected override void OnCollectionItemReplaced(int index, object oldValue, object newValue)
		{
			if (prefabPicker.FindBestPrefab(oldValue) == prefabPicker.FindBestPrefab(newValue))
			{
				SetItemValue(index, newValue);
			}
			else
			{
				RemoveItem(index);
				InsertItem(index, newValue);
			}
		}

		protected virtual CollectionItemViewModelComponent SpawnItem(CollectionItemViewModelComponent prefab, Transform itemParent)
		{
			CollectionItemViewModelComponent result;

			if (pool)
			{
				result = pool.Spawn(prefab, itemParent, false);
			}
			else
			{
				result = Instantiate(prefab, itemParent, false);
			}

			return result;
		}

		protected virtual void DisposeItem(CollectionItemViewModelComponent item)
		{
			if (pool)
			{
				pool.Recycle(item);
			}
			else
			{
				Destroy(item.gameObject);
			}
		}

		private void ClearItems()
		{
			for (int i = currentItems.Count - 1; i >= 0; i--)
			{
				RemoveItem(i);
			}
		}

		private void RemoveItem(int index)
		{
			CollectionItemViewModelComponent item = currentItems[index];
			currentItems.RemoveAt(index);
			DisposeItem(item);
		}

		private void InsertItem(int index, object value)
		{
			CollectionItemViewModelComponent bestPrefab = prefabPicker.FindBestPrefab(value);
			CollectionItemViewModelComponent newItem = SpawnItem(bestPrefab, Container);
			currentItems.Insert(index, newItem);
			newItem.transform.SetSiblingIndex(index);
			SetItemValue(index, value);
		}

		private void SetItemValue(int index, object value)
		{
			currentItems[index].SetData(value);
		}

		private void MoveItem(int oldIndex, int newIndex)
		{
			CollectionItemViewModelComponent item = currentItems[oldIndex];
			currentItems.RemoveAt(oldIndex);
			currentItems.Insert(newIndex, item);
			item.transform.SetSiblingIndex(newIndex);
		}
	}
}
