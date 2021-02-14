namespace Juice
{
	public class StringToFloatOperator : ToOperator<string, float>
	{
		protected override float Convert(string value)
		{
			return float.Parse(value);
		}
	}
}