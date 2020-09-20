using UnityEngine;

namespace Maui
{
	public class ActivateGameObjectBinder : VariableBinder<bool>
	{
		[SerializeField] private GameObject target;

		protected override void Refresh(bool value)
		{
			if (target)
			{
				target.SetActive(value);
			}
		}
	}
}