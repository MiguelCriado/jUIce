using System.Threading.Tasks;
using UnityEngine;

namespace Juice
{
	public class WaitForTrueTransition : ComponentTransition
	{
		[SerializeField] private BindingInfo valueToCheck = BindingInfo.Variable<bool>();

		private VariableBinding<bool> valueToCheckBinding;

		protected override void Awake()
		{
			base.Awake();
			
			valueToCheckBinding = RegisterVariable<bool>(valueToCheck).GetBinding();
		}

		protected override void PrepareInternal(RectTransform target)
		{

		}

		protected override async Task AnimateInternal(RectTransform target)
		{
			while (valueToCheckBinding.IsBound == false || valueToCheckBinding.Property.GetValue(false) == false)
			{
				await Task.Yield();
			}
		}

		protected override void CleanupInternal(RectTransform target)
		{

		}
	}
}