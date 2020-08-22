using System;

namespace Maui.Utils
{
	public static class TypeExtensions
	{
		public static bool ImplementsOrDerives(this Type type, Type baseType)
		{
			bool result = false;

			if (type != null)
			{
				if (baseType.IsGenericType)
				{
					if (baseType.IsGenericTypeDefinition)
					{
						if (baseType.IsInterface)
						{
							int i = 0;
							Type[] interfaces = type.GetInterfaces();

							while (result == false && i < interfaces.Length)
							{
								Type @interface = interfaces[i];
								result = @interface.IsGenericType && @interface.GetGenericTypeDefinition() == baseType;
								i++;
							}
						}
							
						result |= type.IsGenericType && type.GetGenericTypeDefinition() == baseType;
					}
					else
					{
						result = baseType.IsAssignableFrom(type);
					}
				}
				else
				{
					result = baseType.IsAssignableFrom(type);
				}
					
				result |= ImplementsOrDerives(type.BaseType, baseType);
			}

			return result;
		}

		public static Type GetGenericTypeTowardsRoot(this Type type)
		{
			Type result = null;

			if (type != null)
			{
				if (type.IsGenericType)
				{
					result = type;
				}
				else
				{
					result = GetGenericTypeTowardsRoot(type.BaseType);
				}
			}

			return result;
		}
	}
}