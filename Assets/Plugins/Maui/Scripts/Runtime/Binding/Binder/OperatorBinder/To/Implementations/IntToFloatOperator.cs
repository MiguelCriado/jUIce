namespace Maui
{
	public class IntToFloatOperator : ToOperator<int, float>
	{
		protected override float Convert(int value)
		{
			return value;
		}
	}
}