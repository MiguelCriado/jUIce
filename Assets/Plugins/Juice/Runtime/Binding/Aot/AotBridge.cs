using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Scripting;

namespace Juice
{
	public delegate object ViewModelPropertyGetHandler(IViewModel viewModel, string propertyName);

	public static class AotBridge
	{
		private static readonly string AotDynamicTypeName = "Juice.Runtime.Aot.ViewModelResolver, Juice.Runtime.Aot, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";
		private static readonly string InitializeMethodName = "Initialize";

		private static readonly Dictionary<Type, ViewModelPropertyGetHandler> PropertyGetters = new Dictionary<Type, ViewModelPropertyGetHandler>();

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

		public static void RegisterPropertyGetter<T>(ViewModelPropertyGetHandler getter) where T : IViewModel
		{
			PropertyGetters[typeof(T)] = getter;
		}

		public static object GetProperty(IViewModel viewModel, string propertyName)
		{
			object result = null;

			if (PropertyGetters.TryGetValue(viewModel.GetType(), out ViewModelPropertyGetHandler propertyGetter))
			{
				result = propertyGetter(viewModel, propertyName);
			}

			return result;
		}
	}
}
