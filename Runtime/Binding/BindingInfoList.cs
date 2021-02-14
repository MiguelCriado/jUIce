using System;
using System.Collections.Generic;
using UnityEngine;

namespace Juice
{
	[Serializable]
	public class BindingInfoList
	{
		public Type Type => type.Type;
		public IReadOnlyList<BindingInfo> BindingInfos => bindingInfoList;
		
		[SerializeField] protected SerializableType type;
		[SerializeField] protected List<BindingInfo> bindingInfoList = new List<BindingInfo>();

		public BindingInfoList(Type type)
		{
			this.type = new SerializableType(type);
		}
		
		public void AddElement()
		{
			bindingInfoList.Add(new BindingInfo(Type));
		}
	}
}