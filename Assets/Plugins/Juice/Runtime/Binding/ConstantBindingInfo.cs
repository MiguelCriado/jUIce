using System;
using UnityEngine;

namespace Juice
{
	[Serializable]
	public abstract class ConstantBindingInfo : BindingInfo
	{
		public bool UseConstant => useConstant;
		
		[SerializeField] private bool useConstant;

		protected ConstantBindingInfo(Type targetType, bool useConstant) : base(targetType)
		{
			this.useConstant = useConstant;
		}
	}
	
	[Serializable]
	public class ConstantBindingInfo<T> : ConstantBindingInfo
	{
		public T Constant
		{
			get => constant;
			set => constant = value;
		}

		[SerializeField] private T constant;

		public ConstantBindingInfo() : this(false)
		{
			
		}

		public ConstantBindingInfo(bool useConstant, T constantValue = default) 
			: base(typeof(IReadOnlyObservableVariable<T>), useConstant)
		{
			constant = constantValue;
		}
	}
}