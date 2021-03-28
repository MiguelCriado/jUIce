using UnityEngine;

namespace Juice
{
	public class SkipOperator<T> : ProcessorOperator<T, T>
	{
		private static readonly BindingType[] AllowedTypesStatic = 
		{
			BindingType.Variable,
			BindingType.Command,
			BindingType.Event
		};

		[SerializeField] private int skipAmount = 1;

		protected override BindingType[] AllowedTypes => AllowedTypesStatic;

		protected override IBindingProcessor GetBindingProcessor(BindingType bindingType, BindingInfo fromBinding)
		{
			IBindingProcessor result = null;
			
			switch (bindingType)
			{
				case BindingType.Variable:
					result = new SkipVariableBindingProcessor<T>(fromBinding, this, skipAmount);
					break;
				case BindingType.Collection:
					Debug.LogError("Collection skip is not supported", this);
					break;
				case BindingType.Command:
					result = new SkipCommandBindingProcessor<T>(fromBinding, this, skipAmount);
					break;
				case BindingType.Event:
					result = new SkipEventBindingProcessor<T>(fromBinding, this, skipAmount);
					break;
			}

			return result;
		}
	}
}