using System.Threading.Tasks;

namespace Juice
{
	public abstract class ToOperatorAsync<TFrom, TTo> : ProcessorOperator<TFrom, TTo>
	{
		protected abstract Task<TTo> ConvertAsync(TFrom value);

		protected override IBindingProcessor GetBindingProcessor(BindingType bindingType, BindingInfo fromBinding)
		{
			IBindingProcessor result = null;

			switch (bindingType)
			{
				case BindingType.Variable:
					result = new ToVariableBindingAsyncProcessor<TFrom, TTo>(fromBinding, this, ConvertAsync);
					break;
				case BindingType.Collection:
					result = new ToCollectionBindingAsyncProcessor<TFrom, TTo>(fromBinding, this, ConvertAsync);
					break;
				case BindingType.Command:
					result = new ToCommandBindingAsyncProcessor<TFrom, TTo>(fromBinding, this, ConvertAsync);
					break;
				case BindingType.Event:
					result = new ToEventBindingAsyncProcessor<TFrom, TTo>(fromBinding, this, ConvertAsync);
					break;
			}

			return result;
		}
	}
}