using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Maui.Editor
{
	public static class MenuItemMethods
	{
		private const string CreateMenuBasePath = "Assets/Create/Maui/";

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

		[MenuItem("GameObject/UI/Muui/Default UI Frame")]
		public static void CreateDefaultUIFrame()
		{
			UIFrameUtility.CreateDefaultUIFrame();
		}
	}
}
