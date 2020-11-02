using System;
using System.Collections.Generic;
using UnityEngine;

namespace Maui
{
	public delegate void BindingListEventHandler<in T>(int index, T newValue);
	
	[Serializable]
	public class BindingList<T>
	{
		public event BindingListEventHandler<T> VariableChanged;

		public IReadOnlyList<T> Values => values;

		private readonly List<VariableBinding<T>> bindingList;
		private List<T> values;

		public BindingList(Component context, BindingInfoList infoList)
		{
			bindingList = new List<VariableBinding<T>>();
			values = new List<T>();
			
			for (int i = 0; i < infoList.BindingInfos.Count; i++)
			{
				BindingInfo current = infoList.BindingInfos[i];
				var newVariable = new VariableBinding<T>(current, context);
				int index = i;
				newVariable.Property.Changed += newValue => OnBoundVariableChanged(index, newValue);
				bindingList.Add(newVariable);
				values.Add(newVariable.Property.Value);
			}
		}

		public void Bind()
		{
			foreach (VariableBinding<T> current in bindingList)
			{
				current.Bind();
			}
		}

		public void Unbind()
		{
			foreach (VariableBinding<T> current in bindingList)
			{
				current.Unbind();
			}
		}

		protected virtual void OnVariableChanged(int index, T newValue)
		{
			VariableChanged?.Invoke(index, newValue);
		}
		
		private void OnBoundVariableChanged(int index, T newValue)
		{
			values[index] = newValue;
			OnVariableChanged(index, newValue);
		}
	}
}