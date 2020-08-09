namespace Maui
{
	public class ToStringValueConverter : ValueConverter<object, string>
	{
		protected override string Convert(object value)
		{
			return value.ToString();
		}
	}
}