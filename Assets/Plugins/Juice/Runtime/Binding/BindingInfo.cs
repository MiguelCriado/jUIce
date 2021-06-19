using System;
using Juice.Plugins.Juice.Runtime.Utils;
using UnityEngine;

namespace Juice
{
	[Serializable]
	public class BindingInfo : ISerializationCallbackReceiver
	{
		public Type Type => type.Type;

		public ViewModelComponent ViewModelContainer
		{
			get => viewModelContainer;
			set => viewModelContainer = value;
		}

		public string PropertyName => propertyName;
		public bool ForceDynamicBinding => forceDynamicBinding;
		public BindingPath Path => path;

		[SerializeField] private SerializableType type;
		[SerializeField] private ViewModelComponent viewModelContainer;
		[SerializeField] private string propertyName;
		[SerializeField] private bool forceDynamicBinding;
		[SerializeField] private BindingPath path;

		public BindingInfo(Type targetType)
		{
			type = new SerializableType(targetType);
		}

		public static BindingInfo Variable<T>()
		{
			return new BindingInfo(typeof(IReadOnlyObservableVariable<T>));
		}

		public static BindingInfo Collection<T>()
		{
			return new BindingInfo(typeof(IReadOnlyObservableCollection<T>));
		}

		public static BindingInfo Command()
		{
			return new BindingInfo(typeof(IObservableCommand));
		}

		public static BindingInfo Command<T>()
		{
			return new BindingInfo(typeof(IObservableCommand<T>));
		}

		public static BindingInfo Event()
		{
			return new BindingInfo(typeof(IObservableEvent));
		}

		public static BindingInfo Event<T>()
		{
			return new BindingInfo(typeof(IObservableEvent<T>));
		}

		public void OnBeforeSerialize()
		{
			EnsurePath();
		}

		public void OnAfterDeserialize()
		{
			EnsurePath();
		}

		private void EnsurePath()
		{
			if (string.IsNullOrEmpty(propertyName) == false && string.IsNullOrEmpty(path.PropertyName))
			{
				path = new BindingPath(propertyName);
			}
		}
	}
}
