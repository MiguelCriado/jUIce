using System;
using UnityEngine;

namespace Juice
{
	public class IntConstant : Operator
	{
		[SerializeField] private IntConstantBindingInfo value = new IntConstantBindingInfo();

		protected override void Awake()
		{
			base.Awake();

			ViewModel = ExposeVariable<int>(value);
		}

		protected override Type GetInjectionType()
		{
			return typeof(OperatorVariableViewModel<int>);
		}
	}
}
