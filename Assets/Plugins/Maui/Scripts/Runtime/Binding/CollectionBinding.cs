using System;
using UnityEngine;

namespace Maui
{
	public class CollectionBinding<T> : Binding
	{
		public IReadOnlyObservableCollection<T> Property => exposedProperty;

		private readonly ObservableCollection<T> exposedProperty;
		private IReadOnlyObservableCollection<T> boundProperty;
		
		public CollectionBinding(BindingInfo bindingInfo, Component context) : base(bindingInfo, context)
		{
			exposedProperty = new ObservableCollection<T>();
		}

		protected override Type GetBindingType()
		{
			return typeof(IReadOnlyObservableCollection<>);
		}

		protected override void BindProperty(object property)
		{
			boundProperty = property as IReadOnlyObservableCollection<T>;
			
			if (boundProperty != null)
			{
				boundProperty.Reset += BoundPropertyResetHandler;
				boundProperty.CountChanged += BoundPropertyCountChangedHandler;
				boundProperty.ItemAdded += BoundPropertyItemAddedHandler;
				boundProperty.ItemMoved += BoundPropertyItemMovedHandler;
				boundProperty.ItemRemoved += BoundPropertyItemRemovedHandler;
				boundProperty.ItemReplaced += BoundPropertyItemReplacedHandler;

				exposedProperty.Clear();
				
				foreach (var item in boundProperty)
				{
					exposedProperty.Add(item);
				}
			}
			else
			{
				Debug.LogError($"Property type ({property.GetType()}) different from expected type \"{typeof(IReadOnlyObservableCollection<T>)}");
			}
		}

		protected override void UnbindProperty()
		{
			if (boundProperty != null)
			{
				boundProperty.Reset -= BoundPropertyResetHandler;
				boundProperty.CountChanged -= BoundPropertyCountChangedHandler;
				boundProperty.ItemAdded -= BoundPropertyItemAddedHandler;
				boundProperty.ItemMoved -= BoundPropertyItemMovedHandler;
				boundProperty.ItemRemoved -= BoundPropertyItemRemovedHandler;
				boundProperty.ItemReplaced -= BoundPropertyItemReplacedHandler;

				boundProperty = null;
			}
		}

		private void BoundPropertyResetHandler()
		{
			exposedProperty.Clear();
		}
		
		private void BoundPropertyCountChangedHandler(int oldCount, int newCount)
		{
			
		}
		
		private void BoundPropertyItemAddedHandler(int index, T value)
		{
			exposedProperty.Insert(index, value);
		}
		
		private void BoundPropertyItemMovedHandler(int oldIndex, int newIndex, T value)
		{
			exposedProperty.Move(oldIndex, newIndex);
		}
		
		private void BoundPropertyItemRemovedHandler(int index, T value)
		{
			exposedProperty.RemoveAt(index);
		}
		
		private void BoundPropertyItemReplacedHandler(int index, T oldValue, T newValue)
		{
			exposedProperty[index] = newValue;
		}
	}
}