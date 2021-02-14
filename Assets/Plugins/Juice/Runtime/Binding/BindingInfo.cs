using System;
using UnityEngine;

namespace Juice
{
	[Serializable]
	public class BindingInfo
	{
		public Type Type => type.Type;

		public ViewModelComponent ViewModelContainer
		{
			get => viewModelContainer;
			set => viewModelContainer = value;
		}

		public string PropertyName => propertyName;

		[SerializeField] private SerializableType type;
		[SerializeField] private ViewModelComponent viewModelContainer;
		[SerializeField] private string propertyName;

		public BindingInfo(Type targetType)
		{
			type = new SerializableType(targetType);
		}
	}
}