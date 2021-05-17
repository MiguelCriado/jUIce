using System;
using Juice.Plugins.Juice.Runtime.Utils;

namespace Juice
{
	public struct BindingEntry
	{
		public ViewModelComponent ViewModelComponent { get; }
		public string PropertyName { get; }
		public BindingPath Path => path ?? (path = new BindingPath(ViewModelComponent.Id, PropertyName)).Value;
		public bool NeedsToBeBoxed { get; }
		public Type ObservableType { get; }
		public Type GenericArgument { get; }

		private BindingPath? path;

		public BindingEntry(
			ViewModelComponent viewModelComponent,
			string propertyName,
			bool needsToBeBoxed,
			Type observableType,
			Type genericArgument)
		{
			ViewModelComponent = viewModelComponent;
			PropertyName = propertyName;
			NeedsToBeBoxed = needsToBeBoxed;
			ObservableType = observableType;
			GenericArgument = genericArgument;
			path = null;
		}
	}
}
