namespace Maui
{
	public class IntToStringOperator : ToOperator<int, string>
	{
		protected override string Convert(int value)
		{
			return value.ToString();
		}
	}
}