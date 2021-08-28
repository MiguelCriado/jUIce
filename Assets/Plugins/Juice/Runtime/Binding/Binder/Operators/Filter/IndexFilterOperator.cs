using UnityEngine;

namespace Juice
{
	public abstract class IndexFilterOperator<T> : ProcessorOperator<int, T>
	{
		protected override BindingType[] AllowedTypes => new[]
		{
			BindingType.Variable,
			BindingType.Command,
			BindingType.Event
		};

		protected override string FromBindingName => "Index";

		[SerializeField] private BindingInfo collection = BindingInfo.Collection<T>();

		protected IReadOnlyObservableCollection<T> CollectionBinding => collectionBinding.IsBound ? collectionBinding.Property : null;

		private CollectionBinding<T> collectionBinding;

		protected override void Awake()
		{
			base.Awake();

			RegisterCollectionBinding();
		}

		protected override IBindingProcessor GetBindingProcessor(BindingType bindingType, BindingInfo fromBinding)
		{
			IBindingProcessor result = null;

			switch (bindingType)
			{
				case BindingType.Variable:
					result = new IndexFilterVariableBindingProcessor<T>(fromBinding, this, Filter, RegisterCollectionBinding());
					break;
				case BindingType.Command:
					result = new ToCommandBindingProcessor<int, T>(fromBinding, this, Filter);
					break;
				case BindingType.Event:
					result = new ToEventBindingProcessor<int, T>(fromBinding, this, Filter);
					break;
			}

			return result;
		}

		protected virtual T Filter(int index)
		{
			T result = default;

			if (collectionBinding.IsBound
			    && index >= 0
			    && index < collectionBinding.Property.Count)
			{
				result = collectionBinding.Property[index];
			}

			return result;
		}

		private CollectionBinding<T> RegisterCollectionBinding()
		{
			if (collectionBinding == null)
			{
				collectionBinding = RegisterCollection<T>(collection).GetBinding();
			}

			return collectionBinding;
		}
	}
}
