using UnityEngine;
using UnityEngine.UI;

namespace Juice
{
	public class GraphicBinder : VariableBinder<Color>
	{
		private Graphic graphic;
		
		protected override void Awake()
		{
			base.Awake();

			graphic = GetComponent<Graphic>();

			if (!graphic)
			{
				Debug.LogError($"{nameof(GraphicBinder)} requires a {nameof(Graphic)} to work.", this);
			}
		}

		protected override void Refresh(Color value)
		{
			if (graphic)
			{
				graphic.color = value;
			}
		}
	}
}