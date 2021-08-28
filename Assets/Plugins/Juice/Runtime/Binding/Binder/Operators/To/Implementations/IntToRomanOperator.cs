using System.Text;

namespace Juice
{
	public class IntToRomanOperator : ToOperator<int, string>
	{
		private struct PairRomanNumber
		{
			public int Number { get; }
			public string RomanSymbol { get; }

			public PairRomanNumber(int number, string romanSymbol)
			{
				Number = number;
				RomanSymbol = romanSymbol;
			}
		}

		private static readonly PairRomanNumber[] Map = 
		{
			new PairRomanNumber(1000, "M"),
			new PairRomanNumber(900, "CM"),
			new PairRomanNumber(500, "D"),
			new PairRomanNumber(400, "CD"),
			new PairRomanNumber(100, "C"),
			new PairRomanNumber(90, "XC"),
			new PairRomanNumber(50, "L"),
			new PairRomanNumber(40, "XL"),
			new PairRomanNumber(10, "X"),
			new PairRomanNumber(9, "IX"),
			new PairRomanNumber(5, "V"),
			new PairRomanNumber(4, "IV"),
			new PairRomanNumber(1, "I"),
		};

		private static readonly StringBuilder StringBuilder = new StringBuilder();

		protected override string Convert(int value)
		{
			StringBuilder.Clear();
			
			ConvertToRoman(value, StringBuilder);

			return StringBuilder.ToString();
		}

		private void ConvertToRoman(int value, StringBuilder stringBuilder)
		{
			if (value > 0)
			{
				PairRomanNumber entry = GetRomanEntry(value);
				stringBuilder.Append(entry.RomanSymbol);
				ConvertToRoman(value - entry.Number, stringBuilder);
			}
		}

		private PairRomanNumber GetRomanEntry(int value)
		{
			PairRomanNumber result = default;
			bool found = false;
			int i = 0;

			while (found == false && i < Map.Length)
			{
				if (Map[i].Number <= value)
				{
					found = true;
					result = Map[i];
				}
				
				i++;
			}

			return result;
		}
	}
}
