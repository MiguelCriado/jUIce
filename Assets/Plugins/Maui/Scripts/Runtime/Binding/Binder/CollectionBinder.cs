using UnityEngine;

namespace Maui
{
	public abstract class CollectionBinder<T> : MonoBehaviour, IBinder<T>
	{
		[SerializeField] private BindingInfo bindingInfo = new BindingInfo(typeof(IReadOnlyObservableCollection<T>));

		private CollectionBinding<T> binding;

		protected virtual void Awake()
		{
			binding = new CollectionBinding<T>(bindingInfo, this);
			binding.Property.Reset += OnCollectionReset;
			binding.Property.CountChanged += OnCollectionCountChanged;
			binding.Property.ItemAdded += OnCollectionItemAdded;
			binding.Property.ItemMoved += OnCollectionItemMoved;
			binding.Property.ItemRemoved += OnCollectionItemRemoved;
			binding.Property.ItemReplaced += OnCollectionItemReplaced;
		}
		
		protected virtual void OnEnable()
		{
			binding.Bind();
		}

		protected virtual void OnDisable()
		{
			binding.Unbind();
		}
		
		protected abstract void OnCollectionReset();
		protected abstract void OnCollectionCountChanged(int oldCount, int newCount);
		protected abstract void OnCollectionItemAdded(int index, T value);
		protected abstract void OnCollectionItemMoved(int oldIndex, int newIndex, T value);
		protected abstract void OnCollectionItemRemoved(int index, T value);
		protected abstract void OnCollectionItemReplaced(int index, T oldValue, T newValue);
	}
}