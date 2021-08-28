using System;
using System.Collections.Generic;
using Juice.Pooling;
using Juice.Utils;
using UnityEngine;

namespace Juice
{
	public class SpawnPrefabBinder: ComponentBinder
	{
		private Transform Parent => parent ? parent : transform;

		[SerializeField] private BindingInfo objectToInstantiate = BindingInfo.Variable<object>();
		[SerializeField] private List<BindableViewModelComponent> prefabs;
		[SerializeField] private Transform parent;
		[SerializeField] private ObjectPool pool;

		private PrefabPicker<BindableViewModelComponent> prefabPicker;
		private BindableViewModelComponent currentItem;

		protected void Reset()
		{
			pool = GetComponent<ObjectPool>();
		}

		protected virtual void OnValidate()
		{
			if (!parent)
			{
				parent = transform;
			}
		}

		protected override void Awake()
		{
			base.Awake();

			RegisterVariable<object>(objectToInstantiate)
				.OnChanged(OnObjectChanged)
				.OnCleared(OnObjectCleared);

			prefabPicker = new PrefabPicker<BindableViewModelComponent>(prefabs);
			currentItem = null;

			if (!pool)
			{
				pool = this.GetOrAddComponent<ObjectPool>();
			}

			foreach (BindableViewModelComponent prefab in prefabs)
			{
				pool.CreatePool(prefab, 1);
			}
		}

		private void OnObjectChanged(object value)
		{
			BindableViewModelComponent bestPrefab = prefabPicker.FindBestPrefab(value);

			if (bestPrefab)
			{
				if (currentItem == null || bestPrefab.ExpectedType != currentItem.ExpectedType)
				{
					currentItem = SpawnItem(bestPrefab, Parent);
				}

				currentItem.SetData(value);
			}
			else
			{
				Clear();

				Debug.LogError($"A matching prefab could not be found for {value} ({value.GetType().GetPrettifiedName()})");
			}
		}

		private void OnObjectCleared()
		{
			Clear();
		}

		private BindableViewModelComponent SpawnItem(BindableViewModelComponent prefab, Transform parent)
		{
			Clear();

			return pool.Spawn(prefab, parent, false);
		}

		private void Clear()
		{
			if (currentItem != null)
			{
				pool.Recycle(currentItem);
				currentItem = null;
			}
		}
	}
}
