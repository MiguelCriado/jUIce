using System;
using System.Collections.Generic;
using UnityEngine;

namespace Maui
{
	[Serializable]
	public class BindingList
	{
		public Type Type => type.Type;
		
		[SerializeField] private SerializableType type;
		[SerializeField] private List<BindingInfo> bindings = new List<BindingInfo>();

		public BindingList(Type targetType)
		{
			type = new SerializableType(targetType);
		}

		public void AddElement()
		{
			bindings.Add(new BindingInfo(Type));
		}
	}
}