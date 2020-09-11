using Maui.Collections;

namespace Maui
{
	public abstract class MapOperator<TFrom, TTo> : ToOperator<TFrom, TTo>
	{
		protected abstract SerializableDictionary<TFrom, TTo> Mapper { get; }
		
		protected override TTo Convert(TFrom value)
		{
			return Mapper[value];
		}
	}
}