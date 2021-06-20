using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Scripting;

namespace Juice
{
	public static class AotBridge
	{
		private static readonly string AotDynamicTypeName = "Juice.Runtime.Aot.ViewModelResolver, Juice.Runtime.Aot, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";
		private static readonly string InitializeMethodName = "Initialize";

		private static readonly Dictionary<Type, IViewModelPropertyGetter> PropertyGetters = new Dictionary<Type, IViewModelPropertyGetter>();

		public static bool CanGetProperties { get; }

		static AotBridge()
		{
			Type resolverType = Type.GetType(AotDynamicTypeName);

			if (resolverType != null)
			{
				MethodInfo initializeMethod = resolverType.GetMethod(InitializeMethodName);
				initializeMethod?.Invoke(null, null);

				CanGetProperties = initializeMethod != null;
			}
		}

		public static void RegisterPropertyGetter<T>(IViewModelPropertyGetter getter) where T : IViewModel
		{
			PropertyGetters[typeof(T)] = getter;
		}

		public static object GetProperty(IViewModel viewModel, string propertyName)
		{
			object result = null;

			if (PropertyGetters.TryGetValue(viewModel.GetType(), out IViewModelPropertyGetter propertyGetter))
			{
				result = propertyGetter.GetProperty(viewModel, propertyName);
			}

			return result;
		}
	}
}
