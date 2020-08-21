namespace Maui.Sandbox
{
	public class FloatToStringValueConverter : ValueConverter<float, string>
	{
		protected override string Convert(float value)
		{
			return value.ToString();
		}
	}
}