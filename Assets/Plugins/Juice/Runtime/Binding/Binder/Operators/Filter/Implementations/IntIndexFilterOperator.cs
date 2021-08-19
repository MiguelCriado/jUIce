using UnityEngine;

namespace Juice
{
	public class IntIndexFilterOperator : IndexFilterOperator<int>
	{
		[SerializeField] private ConstantBindingInfo<int> outOfBoundsValue = new ConstantBindingInfo<int>();

		private VariableBinding<int> outOfBoundsValueBinding;

		protected override void Awake()
		{
			base.Awake();

			outOfBoundsValueBinding = RegisterVariable<int>(outOfBoundsValue).GetBinding();
		}

		protected override int Filter(int index)
		{
			int result;

			if (CollectionBinding != null
			    && index >= 0
			    && index < CollectionBinding.Count)
			{
				result = CollectionBinding[index];
			}
			else
			{
				result = outOfBoundsValueBinding.Property.GetValue(-1);
			}

			return result;
		}
	}
}
