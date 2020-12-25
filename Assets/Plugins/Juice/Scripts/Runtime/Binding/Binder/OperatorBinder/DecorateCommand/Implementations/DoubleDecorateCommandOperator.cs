using UnityEngine;

namespace Juice
{
	public class DoubleDecorateCommandOperator : DecorateCommandOperator<double>
	{
		protected override ConstantBindingInfo<double> DecorationBindingInfo => decorationBindingInfo;

		[SerializeField] private DoubleConstantBindingInfo decorationBindingInfo = new DoubleConstantBindingInfo();
	}
}