using UnityEngine;

namespace Maui
{
	public abstract class DelayOperator<T> : ProcessorOperatorBinder<T, T>
	{
		[SerializeField] private float delay;
		
		protected override IBindingProcessor GetBindingProcessor(BindingType bindingType, BindingInfo fromBinding)
		{
			IBindingProcessor result = null;
			
			switch (bindingType)
			{
				case BindingType.Variable:
					result = new DelayVariableBindingProcessor<T>(fromBinding, this, delay);
					break;
				case BindingType.Collection:
					result = new DelayCollectionBindingProcessor<T>(fromBinding, this, delay);
					break;
				case BindingType.Command:
					result = new DelayCommandBindingProcessor<T>(fromBinding, this, delay);
					break;
				case BindingType.Event:
					result = new DelayEventBindingProcessor<T>(fromBinding, this, delay);
					break;
			}

			return result;
		}
	}
}