using UnityEngine;

namespace Juice
{
	public abstract class ItemSetter : MonoBehaviour
	{
		public abstract void SetData(int index, GameObject item, object value);
	}
}
