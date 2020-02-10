using System.Collections.Generic;

namespace Muui
{
	public interface IObservableDictionary<TKey, TValue> : IReadOnlyObservableDictionary<TKey, TValue>, IDictionary<TKey, TValue>
	{

	}
}
