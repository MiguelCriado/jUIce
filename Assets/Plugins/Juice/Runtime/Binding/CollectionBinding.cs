using System;
using Juice.Utils;
using UnityEngine;

namespace Juice
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
				boundProperty.Reset += OnBoundPropertyReset;
				boundProperty.CountChanged += OnBoundPropertyCountChanged;
				boundProperty.ItemAdded += OnBoundPropertyItemAdded;
				boundProperty.ItemMoved += OnBoundPropertyItemMoved;
				boundProperty.ItemRemoved += OnBoundPropertyItemRemoved;
				boundProperty.ItemReplaced += OnBoundPropertyItemReplaced;

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
				boundProperty.Reset -= OnBoundPropertyReset;
				boundProperty.CountChanged -= OnBoundPropertyCountChanged;
				boundProperty.ItemAdded -= OnBoundPropertyItemAdded;
				boundProperty.ItemMoved -= OnBoundPropertyItemMoved;
				boundProperty.ItemRemoved -= OnBoundPropertyItemRemoved;
				boundProperty.ItemReplaced -= OnBoundPropertyItemReplaced;

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

		private void OnBoundPropertyReset()
		{
			exposedProperty.Clear();
		}
		
		private void OnBoundPropertyCountChanged(int oldCount, int newCount)
		{
			
		}
		
		private void OnBoundPropertyItemAdded(int index, T value)
		{
			exposedProperty.Insert(index, value);
		}
		
		private void OnBoundPropertyItemMoved(int oldIndex, int newIndex, T value)
		{
			exposedProperty.Move(oldIndex, newIndex);
		}
		
		private void OnBoundPropertyItemRemoved(int index, T value)
		{
			exposedProperty.RemoveAt(index);
		}
		
		private void OnBoundPropertyItemReplaced(int index, T oldValue, T newValue)
		{
			exposedProperty[index] = newValue;
		}
	}
}