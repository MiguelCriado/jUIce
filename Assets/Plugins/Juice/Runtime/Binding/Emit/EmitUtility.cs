using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Juice.Emit
{
	public static class EmitUtility
	{
		public static Type BuildViewModelPropertyGetter(ModuleBuilder moduleBuilder, Type viewModelType)
		{
			List<BindingEntry> bindings = BindingUtils.GetAllBindings(viewModelType).ToList();
			Type getterFuncType = typeof(Func<,>).MakeGenericType(viewModelType, typeof(object));
			Type getterMapType = typeof(Dictionary<,>).MakeGenericType(typeof(string), getterFuncType);

			TypeBuilder typeBuilder = moduleBuilder.DefineType(
				$"{viewModelType.Name}_PropertyGetter",
				TypeAttributes.Public,
				typeof(object),
				new [] {typeof(IViewModelPropertyGetter)});

			FieldBuilder getterMap = typeBuilder.DefineField(
				"getterMap",
				getterMapType,
				FieldAttributes.Private);

			var getters = DefineConcretePropertyGetters(viewModelType, bindings, typeBuilder);
			GenerateConstructor(typeBuilder, bindings, getterMapType, getterMap, getters, getterFuncType);
			GenerateGetPropertyMethod(viewModelType, typeBuilder, getterFuncType, getterMap, getterMapType);
			GenerateConcreteGetPropertyMethods(viewModelType, getters);

			Type returnType = typeBuilder.CreateType();
			return returnType;
		}

		private static Dictionary<BindingEntry, MethodBuilder> DefineConcretePropertyGetters(
			Type viewModelType,
			List<BindingEntry> bindings,
			TypeBuilder typeBuilder)
		{
			var result = new Dictionary<BindingEntry, MethodBuilder>(bindings.Count);

			foreach (BindingEntry current in bindings)
			{
				result[current] = typeBuilder.DefineMethod(
					$"Get{current.Path.PropertyName}",
					MethodAttributes.Private | MethodAttributes.Static,
					typeof(object),
					new[] {viewModelType});
			}

			return result;
		}

		private static void GenerateConstructor(
			TypeBuilder typeBuilder,
			List<BindingEntry> bindings,
			Type getterMapType,
			FieldBuilder getterMap,
			Dictionary<BindingEntry, MethodBuilder> getters,
			Type getterFuncType)
		{
			ConstructorBuilder ctor = typeBuilder.DefineConstructor(
				MethodAttributes.Public,
				CallingConventions.Standard,
				Type.EmptyTypes);

			ILGenerator il = ctor.GetILGenerator();

			il.Emit(OpCodes.Ldarg_0); // this
			il.Emit(OpCodes.Ldc_I4, bindings.Count);
			il.Emit(OpCodes.Newobj, getterMapType.GetConstructor(new[] {typeof(int)}));
			il.Emit(OpCodes.Stfld, getterMap);

			il.Emit(OpCodes.Ldarg_0); // this
			il.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));

			foreach (BindingEntry current in bindings)
			{
				il.Emit(OpCodes.Ldarg_0); // this
				il.Emit(OpCodes.Ldfld, getterMap);
				il.Emit(OpCodes.Ldstr, current.PropertyName);
				il.Emit(OpCodes.Ldnull); // this
				il.Emit(OpCodes.Ldftn, getters[current]);
				il.Emit(OpCodes.Newobj, getterFuncType.GetConstructor(new[] {typeof(object), typeof(IntPtr)}));
				il.Emit(OpCodes.Callvirt, getterMapType.GetMethod("set_Item"));
			}

			il.Emit(OpCodes.Ret);
		}

		private static void GenerateGetPropertyMethod(
			Type viewModelType,
			TypeBuilder typeBuilder,
			Type getterFuncType,
			FieldBuilder getterMap,
			Type getterMapType)
		{
			MethodBuilder getProperty = typeBuilder.DefineMethod(
				nameof(IViewModelPropertyGetter.GetProperty),
				MethodAttributes.Public | MethodAttributes.Virtual,
				typeof(object),
				new[] {typeof(IViewModel), typeof(string)});

			ILGenerator il = getProperty.GetILGenerator();

			LocalBuilder result = il.DeclareLocal(typeof(object));
			LocalBuilder typedTarget = il.DeclareLocal(viewModelType);
			LocalBuilder getter = il.DeclareLocal(getterFuncType);
			LocalBuilder canBeResolved = il.DeclareLocal(typeof(bool));
			LocalBuilder returnValue = il.DeclareLocal(typeof(object));

			Label wrongTypeLabel = il.DefineLabel();
			Label getterNotFoundLabel = il.DefineLabel();
			Label skipIfBodyLabel = il.DefineLabel();
			Label returnLabel = il.DefineLabel();

			il.Emit(OpCodes.Ldnull);
			il.Emit(OpCodes.Stloc, result);

			il.Emit(OpCodes.Ldarg_1); // viewModel
			il.Emit(OpCodes.Isinst, viewModelType);
			il.Emit(OpCodes.Stloc, typedTarget);
			il.Emit(OpCodes.Ldloc, typedTarget);
			il.Emit(OpCodes.Brfalse_S, wrongTypeLabel); // viewmodel is ViewModelType == false

			il.Emit(OpCodes.Ldarg_0); // this
			il.Emit(OpCodes.Ldfld, getterMap);
			il.Emit(OpCodes.Ldarg_2); // propertyName
			il.Emit(OpCodes.Ldloca_S, getter);
			il.Emit(OpCodes.Callvirt, getterMapType.GetMethod("TryGetValue"));
			il.Emit(OpCodes.Br_S, getterNotFoundLabel);
			il.MarkLabel(wrongTypeLabel);
			il.Emit(OpCodes.Ldc_I4_0);
			il.MarkLabel(getterNotFoundLabel);
			il.Emit(OpCodes.Stloc, canBeResolved);

			il.Emit(OpCodes.Ldloc, canBeResolved);
			il.Emit(OpCodes.Brfalse_S, skipIfBodyLabel);

			il.Emit(OpCodes.Ldloc, getter);
			il.Emit(OpCodes.Ldloc, typedTarget);
			il.Emit(OpCodes.Callvirt, getterFuncType.GetMethod("Invoke"));
			il.Emit(OpCodes.Stloc, result);

			il.MarkLabel(skipIfBodyLabel);
			il.Emit(OpCodes.Ldloc, result);
			il.Emit(OpCodes.Stloc_S, returnValue);
			il.Emit(OpCodes.Br_S, returnLabel);

			il.MarkLabel(returnLabel);
			il.Emit(OpCodes.Ldloc_S, returnValue);
			il.Emit(OpCodes.Ret);

			typeBuilder.DefineMethodOverride(
				getProperty,
				typeof(IViewModelPropertyGetter).GetMethod(nameof(IViewModelPropertyGetter.GetProperty)));
		}

		private static void GenerateConcreteGetPropertyMethods(Type viewModelType, Dictionary<BindingEntry, MethodBuilder> getters)
		{
			foreach (var kvp in getters)
			{
				BindingEntry entry = kvp.Key;
				MethodBuilder getConcretePropertyBuilder = kvp.Value;

				ILGenerator il = getConcretePropertyBuilder.GetILGenerator();

				LocalBuilder returnValue = il.DeclareLocal(typeof(object));

				Label returnLabel = il.DefineLabel();

				il.Emit(OpCodes.Ldarg_0); // viewModel
				il.Emit(OpCodes.Callvirt, viewModelType.GetMethod($"get_{entry.PropertyName}"));
				il.Emit(OpCodes.Stloc, returnValue);
				il.Emit(OpCodes.Br_S, returnLabel);

				il.MarkLabel(returnLabel);
				il.Emit(OpCodes.Ldloc, returnValue);
				il.Emit(OpCodes.Ret);
			}
		}
	}
}
