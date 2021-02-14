using System;
using System.Globalization;

namespace Juice
{
	public class DateTimeToStringOperator : ToOperator<DateTime, string>
	{
		protected override string Convert(DateTime value)
		{
			return value.ToString(CultureInfo.CurrentCulture);
		}
	}
}