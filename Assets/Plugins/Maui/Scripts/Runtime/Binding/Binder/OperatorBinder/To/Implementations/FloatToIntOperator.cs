using UnityEngine;

namespace Maui
{
	public class FloatToIntOperator : ToOperator<float, int>
	{
		public enum CrunchMode
		{
			Round,
			Floor,
			Ceil
		}

		[SerializeField] private CrunchMode mode;
		
		protected override int Convert(float value)
		{
			int result;

			switch (mode)
			{
				default:
				case CrunchMode.Round: result = Mathf.RoundToInt(value); break;
				case CrunchMode.Floor: result = Mathf.FloorToInt(value); break;
				case CrunchMode.Ceil: result = Mathf.CeilToInt(value); break;
			}

			return result;
		}
	}
}