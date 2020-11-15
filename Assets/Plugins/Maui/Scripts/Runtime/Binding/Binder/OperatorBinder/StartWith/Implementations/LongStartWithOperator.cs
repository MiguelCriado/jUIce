using UnityEngine;

namespace Maui
{
	public class LongStartWithOperator : StartWithOperator<long>
	{
		protected override ConstantBindingInfo<long> InitialValue => initialValue;

		[SerializeField] private LongConstantBindingInfo initialValue = new LongConstantBindingInfo();
	}
}