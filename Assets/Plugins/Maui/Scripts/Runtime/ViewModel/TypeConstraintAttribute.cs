using System;
using UnityEngine;

namespace Maui
{
	public class TypeConstraintAttribute : PropertyAttribute
	{
		public Type BaseType { get; }

		public TypeConstraintAttribute(Type baseType)
		{
			BaseType = baseType;
		}
	}
}
