using UnityEngine;

namespace Juice
{
	public class EnableMonoBehaviourBinder : VariableBinder<bool>
	{
		[SerializeField] private MonoBehaviour target;
		
		protected override void Refresh(bool value)
		{
			if (target)
			{
				target.enabled = value;
			}
		}
	}
}