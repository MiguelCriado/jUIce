using System.Collections.Generic;

namespace Muui
{
	public delegate void DictionaryAddDelegate<TKey, TValue>(TKey key, TValue value);

	public delegate void DictionaryCountChangeDelegate(int count);

	public delegate void DictionaryRemoveDelegate<TKey, TValue>(TKey key, TValue value);

	public delegate void DictionaryReplaceDelegate<TKey, TValue>(TKey key, TValue oldValue, TValue newValue);

	public delegate void DictionaryResetDelegate();

	public interface IReadOnlyObservableDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
	{
		event DictionaryAddDelegate<TKey, TValue> OnAdd;
		event DictionaryCountChangeDelegate OnCountChange;
		event DictionaryRemoveDelegate<TKey, TValue> OnRemove;
		event DictionaryReplaceDelegate<TKey, TValue> OnReplace;
		event DictionaryResetDelegate OnReset;

		int Count { get; }
		TValue this[TKey index] { get; }

		bool ContainsKey(TKey key);
		bool TryGetValue(TKey key, out TValue value);
	}
}
