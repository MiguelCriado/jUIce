namespace Maui.Sandbox
{
	public class IntToStringValueConverter : ValueConverter<int, string>
	{
		protected override string Convert(int value)
		{
			return value.ToString();
		}
	}
}