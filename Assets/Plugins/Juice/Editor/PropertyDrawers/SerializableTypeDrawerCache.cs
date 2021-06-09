using System;
using System.Collections.Generic;
using Juice.Utils;
using UnityEditor;

namespace Juice.Editor
{
	public class SerializableTypeDrawerCache
	{
		public IReadOnlyDictionary<Type, string> TypeMapByType => typeMapByType;
		public IReadOnlyDictionary<string, Type> TypeMapByString => typeMapByString;
		public string CurrentSelection { get; set; }

		private readonly Dictionary<Type, string> typeMapByType = new Dictionary<Type, string>();
		private readonly Dictionary<string, Type> typeMapByString = new Dictionary<string, Type>();

		public SerializableTypeDrawerCache(TypeConstraintAttribute typeConstraint)
		{
			typeMapByType.Clear();
			typeMapByString.Clear();
			var matchingGroups = new Dictionary<string, List<Type>>();

			foreach (Type current in TypeCache.GetTypesDerivedFrom(typeConstraint.BaseType))
			{
				if (IsMatchingType(current, typeConstraint))
				{
					AddTypeToCache(current, matchingGroups);
				}
			}
		}

		private static bool IsMatchingType(Type type, TypeConstraintAttribute constraint)
		{
			return constraint == null
			        || (DoesMatchInstantiableRestriction(type, constraint)
			            && IsAssignableFrom(constraint.BaseType, type));
		}

		private static bool DoesMatchInstantiableRestriction(Type type, TypeConstraintAttribute constraint)
		{
			return !constraint.ForceInstantiableType ||
			       (!type.IsAbstract && !type.IsInterface && !type.IsGenericTypeDefinition);
		}

		private static bool IsAssignableFrom(Type a, Type b)
		{
			return a.IsAssignableFrom(b) || IsAssignableToGenericType(b, a);
		}

		private static bool IsAssignableToGenericType(Type givenType, Type genericType)
		{
			var interfaceTypes = givenType.GetInterfaces();

			foreach (var it in interfaceTypes)
			{
				if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
				{
					return true;
				}
			}

			if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
			{
				return true;
			}

			Type baseType = givenType.BaseType;

			if (baseType == null)
			{
				return false;
			}

			return IsAssignableToGenericType(baseType, genericType);
		}

		private void AddTypeToCache(Type current, Dictionary<string, List<Type>> matchingGroups)
		{
			string typeId = current.GetPrettifiedName();

			if (matchingGroups.TryGetValue(typeId, out List<Type> group))
			{
				group.Add(current);
				FixMatchingTypeIds(group);
			}
			else
			{
				var newGroup = new List<Type> {current};
				matchingGroups.Add(typeId, newGroup);
				AddTypeToCache(current, typeId);
			}
		}

		private void FixMatchingTypeIds(List<Type> group)
		{
			for (int i = 0; i < group.Count; i++)
			{
				Type target = group[i];

				for (int j = i + 1; j < group.Count; j++)
				{
					Type other = group[j];

					string targetId = GetDistinctName(target, other);
					AddTypeToCache(target, targetId);

					string otherId = GetDistinctName(other, target);
					AddTypeToCache(other, otherId);
				}
			}
		}

		private static string GetDistinctName(Type target, Type other)
		{
			string result = target?.GetPrettifiedName();

			if (target != null && other != null && result == other.GetPrettifiedName())
			{
				string[] targetPath = target.FullName?.Split('.');
				string[] otherPath = other.FullName?.Split('.');

				if (targetPath != null && otherPath != null)
				{
					int targetIndex = targetPath.Length - 2;
					int otherIndex = otherPath.Length - 2;

					while (targetIndex >= 0 && otherIndex >= 0 && targetPath[targetIndex] == otherPath[otherIndex])
					{
						result = $"{targetPath[targetIndex]}.{result}";
						targetIndex--;
						otherIndex--;
					}

					if (targetIndex < 0)
					{
						result = $"{result} ({target.Assembly.GetName().Name})";
					}
					else
					{
						result = $"{targetPath[targetIndex]}.{result}";
					}
				}
			}

			return result;
		}

		private void AddTypeToCache(Type type, string newId)
		{
			if (typeMapByType.TryGetValue(type, out string currentId) == false
			    || newId.Length > currentId.Length)
			{
				typeMapByType[type] = newId;
				typeMapByString[newId] = type;
			}
		}
	}
}
