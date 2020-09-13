using System.Collections.Generic;
using UnityEngine;

namespace Maui
{
	public class AllowedBindingTypesAttribute : PropertyAttribute
	{
		public BindingType[] AllowedTypes { get; }
		public string Descriptor { get; }

		public AllowedBindingTypesAttribute(params BindingType[] allowedTypes)
		{
			List<BindingType> temp = new List<BindingType>();
			
			foreach (BindingType bindingType in allowedTypes)
			{
				if (temp.Contains(bindingType) == false)
				{
					temp.Add(bindingType);
				}
			}

			AllowedTypes = temp.ToArray();
		}
		
		public AllowedBindingTypesAttribute(string descriptor)
		{
			Descriptor = descriptor;
		}
	}
}