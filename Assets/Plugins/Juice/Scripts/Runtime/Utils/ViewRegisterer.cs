using System.Linq;
using UnityEngine;

namespace Juice
{
	public class ViewRegisterer : MonoBehaviour
	{
		[SerializeField] private UIFrame uiFrame;

		private void Awake()
		{
			foreach (var view in GetComponentsInChildren<Component>().OfType<IView>())
			{
				uiFrame.RegisterView(view);
			}
		}
	}
}