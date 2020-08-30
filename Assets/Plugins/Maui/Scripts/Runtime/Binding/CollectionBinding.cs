using System;
using Maui.Utils;
using UnityEngine;

namespace Maui
{
	public class CollectionBinding<T> : Binding
	{
		public override bool IsBound => exposedProperty != null;
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

			if (boundProperty == null)
			{
				boundProperty = BoxCollection(property);
			}
			
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
				Debug.LogError($"Property type ({property.GetType()}) cannot be bound as {typeof(IReadOnlyObservableCollection<T>)}", context);
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
		
		private static IReadOnlyObservableCollection<T> BoxCollection(object collectionToBox)
		{
			IReadOnlyObservableCollection<T> result = null;
			
			Type collectionGenericType = collectionToBox.GetType().GetGenericTypeTowardsRoot();

			if (collectionGenericType != null)
			{
				Type actualType = typeof(T);
				Type boxedType = collectionGenericType.GenericTypeArguments[0];
				Type activationType = typeof(CollectionBoxer<,>).MakeGenericType(actualType, boxedType);
				result = Activator.CreateInstance(activationType, collectionToBox) as IReadOnlyObservableCollection<T>;
			}

			return result;
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