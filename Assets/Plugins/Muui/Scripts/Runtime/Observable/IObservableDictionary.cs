using System.Collections.Generic;

namespace Maui
{
	public interface IObservableDictionary<TKey, TValue> : IReadOnlyObservableDictionary<TKey, TValue>, IDictionary<TKey, TValue>
	{

	}
}
