namespace Maui
{
	public class StringToFloatValueConverter : ValueConverter<string, float>
	{
		protected override float Convert(string value)
		{
			return float.Parse(value);
		}
	}
}