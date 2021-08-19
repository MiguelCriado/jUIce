using System.Linq;
using UnityEngine;
using TMPro;
using Juice.Utils;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Juice
{
	[RequireComponent(typeof(TMP_Dropdown))]
	public class TextMeshProDropdownBinder : ComponentBinder
	{
		[SerializeField] private BindingInfo options = BindingInfo.Collection<string>();
		[SerializeField] private BindingInfo defaultIndex = BindingInfo.Variable<int>();
		[SerializeField] private BindingInfo onValueChangedCommand = BindingInfo.Command<int>();

		private TMP_Dropdown dropdown;
		private CollectionBinding<string> optionsBinding;

		protected override void Awake()
		{
			base.Awake();

			dropdown = GetComponent<TMP_Dropdown>();

			optionsBinding = RegisterCollection<string>(options)
				.OnChanged(OnCollectionChanged)
				.GetBinding();

			RegisterVariable<int>(defaultIndex).OnChanged(OnDefaultIndexChanged);

			RegisterCommand<int>(onValueChangedCommand)
				.AddExecuteTrigger(dropdown.onValueChanged)
				.OnCanExecuteChanged(OnValueCanExecuteChanged);
		}

#if UNITY_EDITOR
		[MenuItem("CONTEXT/TMP_Dropdown/Add Binder")]
		private static void AddBinder(MenuCommand command)
		{
			TMP_Dropdown context = (TMP_Dropdown)command.context;
			context.GetOrAddComponent<TextMeshProDropdownBinder>();
		}
#endif
		
		private void OnDefaultIndexChanged(int newValue)
		{
			dropdown.SetValueWithoutNotify(newValue);
		}

		private void OnCollectionChanged()
		{
			dropdown.options.Clear();
			
			dropdown.AddOptions(optionsBinding.Property.ToList());
		}

		private void OnValueCanExecuteChanged(bool newValue)
		{
			dropdown.interactable = newValue;
		}
	}
}
