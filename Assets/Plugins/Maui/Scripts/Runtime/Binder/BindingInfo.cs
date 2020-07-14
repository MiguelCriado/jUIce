using System;
using UnityEngine;

namespace Maui
{
	[Serializable]
	public class BindingInfo
	{
		public Type Type => type.Type;
		public ViewModelComponent ViewModelContainer => viewModelContainer;
		public string PropertyName => propertyName;

		[SerializeField] private SerializableType type;
		[SerializeField] private ViewModelComponent viewModelContainer;
		[SerializeField] private string propertyName;

		public BindingInfo(Type targetType)
		{
			type = new SerializableType(targetType);
		}

		public IObservableVariable<T> ResolveProperty<T>(Component context)
		{
			// TODO: check that T is equivalent to Type
			IObservableVariable<T> result = null;

			if (viewModelContainer != null)
			{

			}

			return result;
		}
	}
}