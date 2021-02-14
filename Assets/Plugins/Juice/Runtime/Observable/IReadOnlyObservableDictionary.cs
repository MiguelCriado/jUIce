using System.Collections.Generic;

namespace Juice
{
	public delegate void DictionaryAddEventHandler<TKey, TValue>(TKey key, TValue value);

	public delegate void DictionaryCountChangeEventHandler(int count);

	public delegate void DictionaryRemoveEventHandler<TKey, TValue>(TKey key, TValue value);

	public delegate void DictionaryReplaceEventHandler<TKey, TValue>(TKey key, TValue oldValue, TValue newValue);

	public delegate void DictionaryResetEventHandler();

	public interface IReadOnlyObservableDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
	{
		event DictionaryAddEventHandler<TKey, TValue> EntryAdded;
		event DictionaryCountChangeEventHandler CountChanged;
		event DictionaryRemoveEventHandler<TKey, TValue> EntryRemoved;
		event DictionaryReplaceEventHandler<TKey, TValue> EntryReplaced;
		event DictionaryResetEventHandler Reset;

		int Count { get; }
		TValue this[TKey index] { get; }

		bool ContainsKey(TKey key);
		bool TryGetValue(TKey key, out TValue value);
	}
}
