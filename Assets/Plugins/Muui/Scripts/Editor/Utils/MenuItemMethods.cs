using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Muui.Editor
{
	public static class MenuItemMethods
	{
		[MenuItem("Assets/Create/Muui/Panel without Properties")]
		public static void CreatePanelWithoutProperties()
		{
			string saveDirectory = ProjectViewUtility.GetSelectedPathOrFallback();
			string selectedPath = EditorUtility.SaveFilePanel
			(
				"Save new Panel",
				saveDirectory,
				"NewPanel.cs",
				"cs"
			);

			if (selectedPath.Length > 0)
			{
				CreatePanelWithoutPropertiesFile(selectedPath);
			}
		}

		[MenuItem("Assets/Create/Muui/Panel with Properties")]
		public static void CreatePanelWithProperties()
		{
			string saveDirectory = ProjectViewUtility.GetSelectedPathOrFallback();
			string selectedPath = EditorUtility.SaveFilePanel
			(
				"Save new Panel",
				saveDirectory,
				"NewPanel.cs",
				"cs"
			);

			if (selectedPath.Length > 0)
			{
				CreatePanelWithPropertiesFile(selectedPath);
			}
		}

		[MenuItem("GameObject/UI/Muui/Default UI Frame")]
		public static void CreateDefaultUIFrame()
		{
			UIFrameUtility.CreateDefaultUIFrame();
		}

		private static void CreatePanelWithoutPropertiesFile(string path)
		{
			FileInfo panelFileInfo = new FileInfo(path);

			if (panelFileInfo.Exists)
			{
				Debug.LogError("The file already exists! Select a new file name.");
			}
			else
			{
				WritePanelWithoutPropertiesFile(panelFileInfo);
				WritePanelPresenterWithoutPropertiesFile(panelFileInfo);
				AssetDatabase.Refresh();
			}
		}

		private static void WritePanelWithoutPropertiesFile(FileInfo panelFileInfo)
		{
			string panelName = panelFileInfo.Name.Substring(0, panelFileInfo.Name.Length - 3);
			ClassFileWriter.ClassDefinition classDefinition = new ClassFileWriter.ClassDefinition
			{
				Imports = new List<string> {"Muui"},
				Name = panelName,
				Superclass = "BasePanelController"
			};

			ClassFileWriter.WriteClassFile(panelFileInfo, classDefinition);
		}

		private static void WritePanelPresenterWithoutPropertiesFile(FileInfo panelFileInfo)
		{
			string panelName = panelFileInfo.Name.Substring(0, panelFileInfo.Name.Length - 3);
			string presenterName = $"{panelName}Presenter";
			string presenterPath = Path.Combine(panelFileInfo.Directory.FullName, $"{presenterName}.cs");
			FileInfo presenterFileInfo = new FileInfo(presenterPath);

			ClassFileWriter.ClassDefinition classDefinition = new ClassFileWriter.ClassDefinition
			{
				Imports = new List<string> {"Muui"},
				Name = presenterName,
				Superclass = "BasePresenter",
				SuperclassGenerics = new List<string> {panelName}
			};

			ClassFileWriter.WriteClassFile(presenterFileInfo, classDefinition);
		}

		private static void CreatePanelWithPropertiesFile(string path)
		{
			FileInfo panelFileInfo = new FileInfo(path);

			if (panelFileInfo.Exists)
			{
				Debug.LogError("The file already exists! Select a new file name.");
			}
			else
			{
				WritePanelWithPropertiesFile(panelFileInfo);
				WritePanelPresenterWithPropertiesFile(panelFileInfo);
				AssetDatabase.Refresh();
			}
		}

		private static void WritePanelWithPropertiesFile(FileInfo panelFileInfo)
		{
			string panelName = panelFileInfo.Name.Substring(0, panelFileInfo.Name.Length - 3);

			ClassFileWriter.ClassDefinition propertiesDefinition = new ClassFileWriter.ClassDefinition
			{
				Attributes = new List<string> {"Serializable"},
				Name = "Properties",
				Superclass = "PanelProperties"
			};

			ClassFileWriter.ClassDefinition classDefinition = new ClassFileWriter.ClassDefinition
			{
				Imports = new List<string> {"Muui", "System"},
				Name = panelName,
				Superclass = "BasePanelController",
				SuperclassGenerics = new List<string> {$"{panelName}.Properties"},
				InnerClasses = new List<ClassFileWriter.ClassDefinition>{propertiesDefinition}
			};

			ClassFileWriter.WriteClassFile(panelFileInfo, classDefinition);
		}

		private static void WritePanelPresenterWithPropertiesFile(FileInfo panelFileInfo)
		{
			string panelName = panelFileInfo.Name.Substring(0, panelFileInfo.Name.Length - 3);
			string presenterName = $"{panelName}Presenter";
			string presenterPath = Path.Combine(panelFileInfo.Directory.FullName, $"{presenterName}.cs");
			FileInfo presenterFileInfo = new FileInfo(presenterPath);

			ClassFileWriter.ClassDefinition classDefinition = new ClassFileWriter.ClassDefinition
			{
				Imports = new List<string> {"Muui"},
				Name = presenterName,
				Superclass = "BasePresenter",
				SuperclassGenerics = new List<string> {panelName, $"{panelName}.Properties"}
			};

			ClassFileWriter.WriteClassFile(presenterFileInfo, classDefinition);
		}
	}
}
