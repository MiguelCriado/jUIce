using UnityEngine;

namespace Juice
{
	public class IntConstant : ConstantOperator<int>
	{
		protected override ConstantBindingInfo Value => value;

		[SerializeField] private IntConstantBindingInfo value = new IntConstantBindingInfo();
	}
}
