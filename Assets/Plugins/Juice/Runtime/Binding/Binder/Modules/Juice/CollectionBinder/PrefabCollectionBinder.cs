using UnityEngine;

namespace Juice
{
	[RequireComponent(typeof(CollectionItemViewModelComponentPicker))]
	[RequireComponent(typeof(CollectionItemViewModelComponentSetter))]
	public class PrefabCollectionBinder : CollectionBinder
	{
		protected virtual void Reset()
		{
			RetrieveItemHandlers();
		}

		protected override void OnValidate()
		{
			base.OnValidate();

			RetrieveItemHandlers();
		}

		private void RetrieveItemHandlers()
		{
			itemPicker = GetComponent<CollectionItemViewModelComponentPicker>();
			itemSetter = GetComponent<CollectionItemViewModelComponentSetter>();
		}
	}
}
