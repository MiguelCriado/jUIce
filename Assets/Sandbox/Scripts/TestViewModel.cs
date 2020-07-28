using System;

namespace Maui.Sandbox
{
	[Serializable]
	public class TestViewModel : IViewModel
	{
		public IObservableVariable<string> Test { get; }

		public TestViewModel()
		{
			Test = new ObservableVariable<string>("Hello World!");
		}
	}
}
