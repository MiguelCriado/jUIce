using System;
using UnityEngine;

namespace Juice
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

		protected ConstantBindingInfo() : base(typeof(IReadOnlyObservableVariable<T>))
		{
			
		}
	}
}