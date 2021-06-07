using UnityEngine;

namespace Juice
{
	public class CollectionItemViewModelComponentSetter : ItemSetter
	{
		public override void SetData(int index, GameObject item, object value)
		{
			var viewModelComponent = item.GetComponent<CollectionItemViewModelComponent>();

			if (viewModelComponent)
			{
				viewModelComponent.SetData(value);
			}
			else
			{
				Debug.LogError($"Item \"{item.name}\" must contain a {nameof(CollectionItemViewModelComponent)}.", this);
			}
		}
	}
}
