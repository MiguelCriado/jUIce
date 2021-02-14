using System.Linq;
using UnityEngine;

namespace Juice
{
	public class ViewRegisterer : MonoBehaviour
	{
		[SerializeField] private UiFrame uiFrame;

		private void Start()
		{
			foreach (var view in GetComponentsInChildren<Component>().OfType<IView>())
			{
				uiFrame.RegisterView(view);
			}
		}
	}
}