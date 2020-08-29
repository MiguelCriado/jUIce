using System;
using UnityEngine;

namespace Maui
{
	[Serializable]
	public abstract class ConstantBindingInfo : BindingInfo
	{
		public bool UseConstant => useConstant;
		
		[SerializeField] private bool useConstant;

		protected ConstantBindingInfo(Type targetType) : base(targetType)
		{
			
		}
	}
	
	[Serializable]
	public abstract class ConstantBindingInfo<T> : ConstantBindingInfo
	{
		public T Constant => constant;
		
		[SerializeField] private T constant;
		
		public ConstantBindingInfo() : base(typeof(IReadOnlyObservableVariable<T>))
		{
			
		}
	}
}