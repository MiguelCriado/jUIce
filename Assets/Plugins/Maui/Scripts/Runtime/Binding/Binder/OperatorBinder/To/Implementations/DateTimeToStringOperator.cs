using System;
using System.Globalization;

namespace Maui
{
	public class DateTimeToStringOperator : ToOperator<DateTime, string>
	{
		protected override string Convert(DateTime value)
		{
			return value.ToString(CultureInfo.CurrentCulture);
		}
	}
}