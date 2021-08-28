namespace Juice
{
	public class LongToIntOperator : ToOperator<long, int>
	{
		protected override int Convert(long value)
		{
			return (int)value;
		}
	}
}