namespace Maui
{
	public class ToStringOperator : ToOperator<object, string>
	{
		protected override string Convert(object value)
		{
			return value.ToString();
		}
	}
}