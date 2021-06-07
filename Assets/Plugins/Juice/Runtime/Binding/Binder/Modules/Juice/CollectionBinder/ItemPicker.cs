using UnityEngine;

namespace Juice
{
	public abstract class ItemPicker : MonoBehaviour
	{
		public abstract GameObject SpawnItem(int index, object value, Transform parent);
		public abstract GameObject ReplaceItem(int index, object oldValue, object newValue, GameObject currentItem, Transform parent);
		public abstract void DisposeItem(int index, GameObject item);
	}
}
