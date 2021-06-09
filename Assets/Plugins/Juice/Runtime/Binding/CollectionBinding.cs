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
				boundProperty = BoxCollection(property, context);
			}

			if (boundProperty != null)
			{
				boundProperty.Reset += OnBoundPropertyReset;
				boundProperty.ItemAdded += OnBoundPropertyItemAdded;
				boundProperty.ItemMoved += OnBoundPropertyItemMoved;
				boundProperty.ItemRemoved += OnBoundPropertyItemRemoved;
				boundProperty.ItemReplaced += OnBoundPropertyItemReplaced;

				exposedProperty.Set(boundProperty);
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
				boundProperty.ItemAdded -= OnBoundPropertyItemAdded;
				boundProperty.ItemMoved -= OnBoundPropertyItemMoved;
				boundProperty.ItemRemoved -= OnBoundPropertyItemRemoved;
				boundProperty.ItemReplaced -= OnBoundPropertyItemReplaced;

				boundProperty = null;
				exposedProperty.Clear();
			}
		}

		private static IReadOnlyObservableCollection<T> BoxCollection(object collectionToBox, Component context)
		{
			IReadOnlyObservableCollection<T> result = null;

			Type collectionGenericType = collectionToBox.GetType().GetGenericTypeTowardsRoot();

			if (collectionGenericType != null)
			{
				try
				{
					Type exposedType = typeof(T);
					Type boxedType = collectionGenericType.GenericTypeArguments[0];
					Type activationType = typeof(CollectionBoxer<,>).MakeGenericType(exposedType, boxedType);
					result = Activator.CreateInstance(activationType, collectionToBox) as IReadOnlyObservableCollection<T>;
				}
#pragma warning disable 618
				catch (ExecutionEngineException)
#pragma warning restore 618
				{
					Debug.LogError($"AOT code not generated to box {typeof(IReadOnlyObservableCollection<T>).GetPrettifiedName()}. " +
					               $"You must force the compiler to generate a CollectionBoxer by using " +
					               $"\"{nameof(AotHelper)}.{nameof(AotHelper.EnsureType)}<{typeof(T).GetPrettifiedName()}>();\" " +
					               $"anywhere in your code.\n" +
					               $"Context: {GetContextPath(context)}", context);
				}
			}

			return result;
		}

		private void OnBoundPropertyReset()
		{
			exposedProperty.Clear();
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
