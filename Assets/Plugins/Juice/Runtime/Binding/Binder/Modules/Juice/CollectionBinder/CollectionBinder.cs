using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Juice
{
	public class CollectionBinder : ComponentBinder
	{
		public IReadOnlyList<GameObject> Items => currentItems;

		[SerializeField] private BindingInfo collection = BindingInfo.Collection<object>();
		[SerializeField] private Transform itemsContainer = default;
		[Header("Dependencies")]
		[SerializeField] protected ItemPicker itemPicker = default;
		[SerializeField] protected ItemSetter itemSetter = default;

		private Transform Container => itemsContainer ? itemsContainer : transform;

		private readonly List<GameObject> currentItems = new List<GameObject>();

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

			Assert.IsNotNull(itemPicker, $"A {nameof(CollectionBinder)} needs an {nameof(ItemPicker)} to work.");
			Assert.IsNotNull(itemSetter, $"A {nameof(CollectionBinder)} needs an {nameof(ItemSetter)} to work.");

			RegisterCollection<object>(collection)
				.OnReset(OnCollectionReset)
				.OnItemAdded(OnCollectionItemAdded)
				.OnItemMoved(OnCollectionItemMoved)
				.OnItemRemoved(OnCollectionItemRemoved)
				.OnItemReplaced(OnCollectionItemReplaced);
		}

		protected virtual void OnCollectionReset()
		{
			ClearItems();
		}

		protected virtual void OnCollectionItemAdded(int index, object value)
		{
			InsertItem(index, value);
		}

		protected virtual void OnCollectionItemMoved(int oldIndex, int newIndex, object value)
		{
			MoveItem(oldIndex, newIndex);
		}

		protected virtual void OnCollectionItemRemoved(int index, object value)
		{
			RemoveItem(index);
		}

		protected virtual void OnCollectionItemReplaced(int index, object oldValue, object newValue)
		{
			currentItems[index] = itemPicker.ReplaceItem(
				index,
				oldValue,
				newValue,
				currentItems[index],
				Container);
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
			GameObject item = currentItems[index];
			currentItems.RemoveAt(index);
			itemPicker.DisposeItem(index, item);
		}

		private void InsertItem(int index, object value)
		{
			GameObject newItem = itemPicker.SpawnItem(index, value, Container);
			currentItems.Insert(index, newItem);
			newItem.transform.SetSiblingIndex(index);
			itemSetter.SetData(index, currentItems[index], value);
		}

		private void MoveItem(int oldIndex, int newIndex)
		{
			GameObject item = currentItems[oldIndex];
			currentItems.RemoveAt(oldIndex);
			currentItems.Insert(newIndex, item);
			item.transform.SetSiblingIndex(newIndex);
		}
	}
}
