using System;
using System.Collections.Generic;
using System.Text;

namespace Juice.Utils
{
	public static class TypeExtensions
	{
		private static readonly StringBuilder StringBuilder = new StringBuilder();

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

		public static Type GetGenericClassTowardsRoot(this Type type)
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
					result = GetGenericClassTowardsRoot(type.BaseType);
				}
			}

			return result;
		}

		public static IEnumerable<Type> GetGenericInterfacesTowardsRoot(this Type type)
		{
			if (type != null)
			{
				if (type.IsInterface && type.IsGenericType)
				{
					yield return type;
				}

				foreach (Type current in type.GetInterfaces())
				{
					if (current.IsGenericType)
					{
						yield return current;
					}
				}

				if (type.BaseType != null)
				{
					foreach (Type current in type.BaseType.GetGenericInterfacesTowardsRoot())
					{
						yield return current;
					}
				}
			}
		}

		public static string GetPrettifiedName(this Type t)
		{
			string result;

			if (t == typeof(bool))
			{
				result = "bool";
			}
			else if (t == typeof(byte))
			{
				result = "byte";
			}
			else if (t == typeof(sbyte))
			{
				result = "sbyte";
			}
			else if (t == typeof(char))
			{
				result = "char";
			}
			else if (t == typeof(decimal))
			{
				result = "decimal";
			}
			else if (t == typeof(float))
			{
				result = "float";
			}
			else if (t == typeof(double))
			{
				result = "double";
			}
			else if (t == typeof(int))
			{
				result = "int";
			}
			else if (t == typeof(uint))
			{
				result = "uint";
			}
			else if (t == typeof(long))
			{
				result = "long";
			}
			else if (t == typeof(ulong))
			{
				result = "ulong";
			}
			else if (t == typeof(short))
			{
				result = "short";
			}
			else if (t == typeof(ushort))
			{
				result = "ushort";
			}
			else if (t == typeof(string))
			{
				result = "string";
			}
			else if (t == typeof(object))
			{
				result = "object";
			}
			else if (t.IsGenericType)
			{
				StringBuilder.Length = 0;

				string name = t.GetGenericTypeDefinition().Name;
				StringBuilder.Append(name.Remove(name.Length - 2));
				StringBuilder.Append("<");

				Type[] typeParameters = t.GetGenericArguments();

				for (int i = 0; i < typeParameters.Length; i++)
				{
					StringBuilder.Append(typeParameters[i].GetPrettifiedName());

					if (i < typeParameters.Length - 1)
					{
						StringBuilder.Append(", ");
					}
				}

				StringBuilder.Append(">");
				result = StringBuilder.ToString();
			}
			else
			{
				result = t.Name;
			}

			return result;
		}
	}
}
