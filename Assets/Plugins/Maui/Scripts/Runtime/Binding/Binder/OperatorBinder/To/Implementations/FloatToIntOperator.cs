using System;
using UnityEngine;

namespace Maui
{
	[Serializable]
	public class FloatToIntOperator : ToOperator<float, int>
	{
		public enum CrunchMode
		{
			Round,
			Floor,
			Ceil
		}

		public CrunchMode Mode;
		
		protected override int Convert(float value)
		{
			int result;

			switch (Mode)
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