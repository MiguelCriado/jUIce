namespace Juice
{
	public class BoolInvertOperator : ToOperator<bool, bool>
	{
		protected override bool Convert(bool value)
		{
			return !value;
		}
	}
}
