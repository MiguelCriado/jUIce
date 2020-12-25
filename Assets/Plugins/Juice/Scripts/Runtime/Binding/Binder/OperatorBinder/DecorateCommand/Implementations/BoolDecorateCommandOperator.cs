using UnityEngine;

namespace Juice
{
	public class BoolDecorateCommandOperator : DecorateCommandOperator<bool>
	{
		protected override ConstantBindingInfo<bool> DecorationBindingInfo => decorationBindingInfo;

		[SerializeField] private BoolConstantBindingInfo decorationBindingInfo = new BoolConstantBindingInfo();
	}
}