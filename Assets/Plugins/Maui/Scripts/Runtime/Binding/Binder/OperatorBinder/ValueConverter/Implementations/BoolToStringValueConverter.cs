namespace Maui
{
	public class BoolToStringValueConverter : ValueConverter<bool, string>
	{
		protected override string Convert(bool value)
		{
			return value.ToString();
		}
	}
}