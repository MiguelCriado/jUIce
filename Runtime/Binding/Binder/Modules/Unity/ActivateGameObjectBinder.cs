using UnityEngine;

namespace Juice
{
	public class ActivateGameObjectBinder : VariableBinder<bool>
	{
		[SerializeField] private GameObject target;

		protected override string BindingInfoName { get; } = "Set Active";

		protected override void Refresh(bool value)
		{
			if (target)
			{
				target.SetActive(value);
			}
		}
	}
}
