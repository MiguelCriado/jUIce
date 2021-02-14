using System.Collections.Generic;

namespace Juice
{
	public interface IObservableDictionary<TKey, TValue> : IReadOnlyObservableDictionary<TKey, TValue>, IDictionary<TKey, TValue>
	{

	}
}
