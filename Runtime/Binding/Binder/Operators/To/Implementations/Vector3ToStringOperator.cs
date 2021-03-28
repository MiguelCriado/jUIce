using UnityEngine;

namespace Juice
{
	public class Vector3ToStringOperator : ToOperator<Vector3, string>
	{
		protected override string Convert(Vector3 value)
		{
			return value.ToString();
		}
	}
}