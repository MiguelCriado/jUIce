using UnityEditor;

namespace Juice.Editor
{
	public static class MenuItemMethods
	{
		private const string CreateMenuBasePath = "Assets/Create/jUIce/";

		[MenuItem(CreateMenuBasePath + "Window")]
		public static void CreateWindow()
		{
			ViewCreationWizardEditorWindow.ShowForWindow();
		}

		[MenuItem(CreateMenuBasePath + "Panel")]
		public static void CreatePanel()
		{
			ViewCreationWizardEditorWindow.ShowForPanel();
		}

		[MenuItem("GameObject/UI/jUIce/Default UI Frame")]
		public static void CreateDefaultUIFrame()
		{
			UIFrameUtility.CreateDefaultUIFrame();
		}
	}
}
