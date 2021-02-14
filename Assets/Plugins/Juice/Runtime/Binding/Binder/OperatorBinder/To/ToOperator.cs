namespace Juice
{
	public abstract class ToOperator<TFrom, TTo> : ProcessorOperatorBinder<TFrom, TTo>
	{
		protected abstract TTo Convert(TFrom value);
		
		protected override IBindingProcessor GetBindingProcessor(BindingType bindingType, BindingInfo fromBinding)
		{
			IBindingProcessor result = null;
			
			switch (bindingType)
			{
				case BindingType.Variable:
					result = new ToVariableBindingProcessor<TFrom, TTo>(fromBinding, this, Convert);
					break;
				case BindingType.Collection:
					result = new ToCollectionBindingProcessor<TFrom, TTo>(fromBinding, this, Convert);
					break;
				case BindingType.Command:
					result = new ToCommandBindingProcessor<TFrom, TTo>(fromBinding, this, Convert);
					break;
				case BindingType.Event:
					result = new ToEventBindingProcessor<TFrom, TTo>(fromBinding, this, Convert);
					break;
			}

			return result;
		}
	}
}