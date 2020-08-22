using System;
using System.Reflection;
using UnityEngine;

namespace Maui
{
	[RequireComponent(typeof(ViewModelComponent))]
	public class CollectionItem : MonoBehaviour, IViewModelInjector
	{
		public Type InjectionType => expectedType.Type;
		public ViewModelComponent Target => target;
		
		[TypeConstraint(typeof(ViewModel<>), true)]
		[SerializeField] private SerializableType expectedType;
		[SerializeField] private ViewModelComponent target;

		protected virtual void Reset()
		{
			target = GetComponent<ViewModelComponent>();
		}

		protected virtual void OnValidate()
		{
			if (target == null)
			{
				target = GetComponent<ViewModelComponent>();
			}

			BindingFlags bindingFlags =
				BindingFlags.Public
				| BindingFlags.NonPublic
				| BindingFlags.Static
				| BindingFlags.Instance
				| BindingFlags.DeclaredOnly;

			FieldInfo expectedTypeInfo = typeof(ViewModelComponent).GetField("expectedType", bindingFlags);
			
			if (expectedTypeInfo != null && expectedTypeInfo.GetValue(target) is SerializableType serializableType)
			{
				serializableType.Type = InjectionType;
			}
		}

		public void SetData(object data)
		{
			if (expectedType.Type != null)
			{
				object viewModel = Activator.CreateInstance(expectedType.Type, data);
				target.ViewModel = (IViewModel)viewModel;
			}
			else
			{
				Debug.LogError("Expected Type must be set", this);
			}
		}
	}
}