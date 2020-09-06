using System;
using System.Collections.Generic;
using UnityEngine;

namespace Maui
{
	public class PrefabCollectionBinder : CollectionBinder<object>
	{
		[SerializeField] private List<CollectionItemViewModelComponent> prefabs;

		private Transform container;
		private List<CollectionItemViewModelComponent> currentItems;

		protected override void Awake()
		{
			base.Awake();

			container = transform;
			currentItems = new List<CollectionItemViewModelComponent>();
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
			SetItemValue(index, newValue);
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
			Destroy(item.gameObject);
			currentItems.RemoveAt(index);
		}

		private void InsertItem(int index, object value)
		{
			CollectionItemViewModelComponent bestPrefab = FindBestPrefab(value);
			CollectionItemViewModelComponent newItem = Instantiate(bestPrefab, container, false);
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

		private CollectionItemViewModelComponent FindBestPrefab(object value)
		{
			CollectionItemViewModelComponent result = null;
			Type valueType = value.GetType();
			int bestDepth = -1;

			foreach (CollectionItemViewModelComponent prefab in prefabs)
			{
				Type injectionType = prefab.InjectionType;
				Type viewModelType;
				
				if (injectionType != null
				    && (viewModelType = GetViewModelType(injectionType)) != null
				    && viewModelType.GenericTypeArguments[0].IsAssignableFrom(valueType))
				{
					Type dataType = viewModelType.GenericTypeArguments[0];
					Type baseType = dataType.BaseType;
					int depth = 0;

					while (baseType != null)
					{
						depth++;
						baseType = baseType.BaseType;
					}

					if (depth > bestDepth)
					{
						bestDepth = depth;
						result = prefab;
					}
				}
			}

			return result;
		}

		private Type GetViewModelType(Type runtimeType)
		{
			Type result = null;

			Type genericType = null;
			
			if (runtimeType.IsGenericType)
			{
				genericType = runtimeType.GetGenericTypeDefinition();
			}

			if (genericType != null && genericType == typeof(BindableViewModel<>))
			{
				result = runtimeType;
			}
			else if (runtimeType.BaseType != null)
			{
				result = GetViewModelType(runtimeType.BaseType);
			}

			return result;
		}
	}
}