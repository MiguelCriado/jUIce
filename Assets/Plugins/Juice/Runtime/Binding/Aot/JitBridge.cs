using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Juice.Emit;

namespace Juice
{
	public static class JitBridge
	{
		private static readonly Dictionary<Type, IViewModelPropertyGetter> GettersMap = new Dictionary<Type, IViewModelPropertyGetter>();

		public static bool IsAvailable { get; }

		private static readonly AssemblyBuilder AssemblyBuilder;
		private static readonly ModuleBuilder ModuleBuilder;

		static JitBridge()
		{
			try
			{
				AppDomain domain = AppDomain.CurrentDomain;
				AssemblyName jitAssemblyName = new AssemblyName("Juice.Runtime.Jit");
				AssemblyBuilder = domain.DefineDynamicAssembly(jitAssemblyName, AssemblyBuilderAccess.Run);
				ModuleBuilder = AssemblyBuilder.DefineDynamicModule(jitAssemblyName.Name);

				IsAvailable = true;
			}
			catch (Exception)
			{
				IsAvailable = false;
			}
		}

		public static object GetProperty(IViewModel viewModel, string propertyName)
		{
			if (GettersMap.ContainsKey(viewModel.GetType()) == false)
			{
				Type getterType = CreateGetterType(viewModel);
				GettersMap[viewModel.GetType()] = Activator.CreateInstance(getterType) as IViewModelPropertyGetter;
			}

			return GettersMap[viewModel.GetType()].GetProperty(viewModel, propertyName);
		}

		private static Type CreateGetterType(IViewModel viewModel)
		{
			return EmitUtility.BuildViewModelPropertyGetter(ModuleBuilder, viewModel.GetType());
		}
	}
}
