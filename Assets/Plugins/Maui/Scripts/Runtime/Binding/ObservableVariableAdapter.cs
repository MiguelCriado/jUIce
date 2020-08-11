using System;
using System.Collections.Generic;

namespace Maui
{
	public class ObservableVariableAdapter : IReadOnlyObservableVariable<object>
	{
		public static HashSet<Type> SupportedTypes = new HashSet<Type>
		{
			typeof(bool),
			typeof(int),
			typeof(long),
			typeof(float),
			typeof(double)
		};
		
		private enum VariableType
		{
			None,
			Bool,
			Int,
			Long,
			Float,
			Double
		}
		
		public event ObservableVariableEventHandler<object> Changed;

		public bool HasValue
		{
			get
			{
				switch (variableType)
				{
					case VariableType.Bool: return boolVariable.HasValue;
					case VariableType.Int: return intVariable.HasValue;
					case VariableType.Long: return longVariable.HasValue;
					case VariableType.Float: return floatVariable.HasValue;
					case VariableType.Double: return doubleVariable.HasValue;
					default: return false;
				}
			}
		}
		
		public object Value 
		{
			get
			{
				switch (variableType)
				{
					case VariableType.Bool: return boolVariable.Value;
					case VariableType.Int: return intVariable.Value;
					case VariableType.Long: return longVariable.Value;
					case VariableType.Float: return floatVariable.Value;
					case VariableType.Double: return doubleVariable.Value;
					default: return null;
				}
			}
		}

		private readonly VariableType variableType;
		private readonly IReadOnlyObservableVariable<bool> boolVariable;
		private readonly IReadOnlyObservableVariable<int> intVariable;
		private readonly IReadOnlyObservableVariable<long> longVariable;
		private readonly IReadOnlyObservableVariable<float> floatVariable;
		private readonly IReadOnlyObservableVariable<double> doubleVariable;

		public ObservableVariableAdapter(object wrappedVariable)
		{
			Type wrappedType = wrappedVariable.GetType();

			if (BindingUtils.CanBeAdapted(wrappedType, typeof(IReadOnlyObservableVariable<object>)))
			{
				Type genericParameter = wrappedType.GenericTypeArguments[0];

				if (genericParameter == typeof(bool))
				{
					variableType = VariableType.Bool;
					boolVariable = (IReadOnlyObservableVariable<bool>) wrappedVariable;
					boolVariable.Changed += BoolVariableChangedHandler;
				}
				else if (genericParameter == typeof(int))
				{
					variableType = VariableType.Int;
					intVariable = (IReadOnlyObservableVariable<int>) wrappedVariable;
					intVariable.Changed += IntVariableChangedHandler;
				}
				else if (genericParameter == typeof(long))
				{
					variableType = VariableType.Long;
					longVariable = (IReadOnlyObservableVariable<long>) wrappedVariable;
					longVariable.Changed += LongVariableChangedHandler;
				}
				else if (genericParameter == typeof(float))
				{
					variableType = VariableType.Float;
					floatVariable = (IReadOnlyObservableVariable<float>) wrappedVariable;
					floatVariable.Changed += FloatVariableChangedHandler;
				}
				else if (genericParameter == typeof(double))
				{
					variableType = VariableType.Double;
					doubleVariable = (IReadOnlyObservableVariable<double>) wrappedVariable;
					doubleVariable.Changed += DoubleVariableChangedHandler;
				}
			}
		}

		~ObservableVariableAdapter()
		{
			switch (variableType)
			{
				case VariableType.Bool: boolVariable.Changed -= BoolVariableChangedHandler; break;
				case VariableType.Int: intVariable.Changed -= IntVariableChangedHandler; break;
				case VariableType.Long: longVariable.Changed -= LongVariableChangedHandler; break;
				case VariableType.Float: floatVariable.Changed -= FloatVariableChangedHandler; break;
				case VariableType.Double: doubleVariable.Changed -= DoubleVariableChangedHandler; break;
			}
		}

		private void BoolVariableChangedHandler(bool newValue)
		{
			Changed?.Invoke(newValue);
		}
		
		private void IntVariableChangedHandler(int newValue)
		{
			Changed?.Invoke(newValue);
		}
		
		private void LongVariableChangedHandler(long newValue)
		{
			Changed?.Invoke(newValue);
		}
		
		private void FloatVariableChangedHandler(float newValue)
		{
			Changed?.Invoke(newValue);
		}
		
		private void DoubleVariableChangedHandler(double newValue)
		{
			Changed?.Invoke(newValue);
		}
	}
}