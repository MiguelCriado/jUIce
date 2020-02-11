using System;

namespace Muui.Tests
{
	public class WindowWithProperties : BaseWindowController<WindowWithProperties.Properties>
	{
		[Serializable]
		public class Properties : WindowProperties
		{
			public IObservableVariable<int> MyIntVariable;
		}
	}
}
