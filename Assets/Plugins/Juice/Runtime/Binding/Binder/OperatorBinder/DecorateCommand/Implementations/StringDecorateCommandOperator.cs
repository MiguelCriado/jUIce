using UnityEngine;

namespace Juice
{
	public class StringDecorateCommandOperator : DecorateCommandOperator<string>
	{
		protected override ConstantBindingInfo<string> DecorationBindingInfo => decorationBindingInfo;

		[SerializeField] private StringConstantBindingInfo decorationBindingInfo = new StringConstantBindingInfo();
	}
}