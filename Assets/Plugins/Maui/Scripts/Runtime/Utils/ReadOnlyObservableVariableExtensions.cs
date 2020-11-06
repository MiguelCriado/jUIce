namespace Maui
{
	public static class ReadOnlyObservableVariableExtensions
	{
		public static T GetValue<T>(this IReadOnlyObservableVariable<T> variable, T fallback)
		{
			T result = fallback;
			
			if (variable.HasValue)
			{
				result = variable.Value;
			}

			return result;
		}
	}
}