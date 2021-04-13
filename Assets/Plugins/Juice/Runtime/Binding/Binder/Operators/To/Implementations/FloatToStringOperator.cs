namespace Juice
{
	public class FloatToStringOperator : ToOperator<float, string>
	{
		protected override string Convert(float value)
		{
			return value.ToString();
		}
	}
}