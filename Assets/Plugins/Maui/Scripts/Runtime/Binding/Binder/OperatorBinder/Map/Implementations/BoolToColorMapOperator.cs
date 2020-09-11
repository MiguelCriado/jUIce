using Maui.Collections;
using UnityEngine;

namespace Maui
{
	public class BoolToColorMapOperator : MapOperator<bool, Color>
	{
		protected override SerializableDictionary<bool, Color> Mapper => mapper;

		[SerializeField] private BoolColorDictionary mapper;
	}
}