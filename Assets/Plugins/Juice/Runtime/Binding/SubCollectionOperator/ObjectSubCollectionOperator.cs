using UnityEngine;

namespace Juice
{
	public class ObjectSubCollectionOperator : SubCollectionOperator<object>
	{
		protected override BindingInfo Collection => collection;

		[SerializeField] private BindingInfo collection = BindingInfo.Collection<object>();
	}
}
