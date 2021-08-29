namespace Juice
{
	public class PayloadEntry<T>
	{
		public bool Exists => window.GetFromPayload<T>(key, out _);

		private readonly string key;
		private readonly IWindow window;

		public PayloadEntry(string key, IWindow window)
		{
			this.key = key;
			this.window = window;
		}

		public T GetValue(T fallback = default)
		{
			if (window.GetFromPayload(key, out T result) == false)
			{
				result = fallback;
			}

			return result;
		}

		public void Remove()
		{
			window.RemoveFromPayload(key);
		}

		public T Consume(T fallback = default)
		{
			T result = GetValue(fallback);
			Remove();
			return result;
		}
	}
}