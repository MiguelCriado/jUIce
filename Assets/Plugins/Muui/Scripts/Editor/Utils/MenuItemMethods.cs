using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Muui.Editor
{
	public static class MenuItemMethods
	{
		private const string CreateMenuBasePath = "Assets/Create/Muui/";

		[MenuItem(CreateMenuBasePath + "Panel without Properties")]
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

		[MenuItem(CreateMenuBasePath + "Panel with Properties")]
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

		[MenuItem(CreateMenuBasePath + "Window without Properties")]
		public static void CreateWindowWithoutProperties()
		{
			string saveDirectory = ProjectViewUtility.GetSelectedPathOrFallback();
			string selectedPath = EditorUtility.SaveFilePanel
			(
				"Save new Window",
				saveDirectory,
				"NewWindow.cs",
				"cs"
			);

			if (selectedPath.Length > 0)
			{
				CreateWindowWithoutPropertiesFile(selectedPath);
			}
		}

		[MenuItem(CreateMenuBasePath + "Window with Properties")]
		public static void CreateWindowWithProperties()
		{
			string saveDirectory = ProjectViewUtility.GetSelectedPathOrFallback();
			string selectedPath = EditorUtility.SaveFilePanel
			(
				"Save new Window",
				saveDirectory,
				"NewWindow.cs",
				"cs"
			);

			if (selectedPath.Length > 0)
			{
				CreateWindowWithPropertiesFile(selectedPath);
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
				WriteScreenPresenterWithoutPropertiesFile(panelFileInfo);
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

		private static void WriteScreenPresenterWithoutPropertiesFile(FileInfo screenFileInfo)
		{
			string screenName = screenFileInfo.Name.Substring(0, screenFileInfo.Name.Length - 3);
			string presenterName = $"{screenName}Presenter";
			string presenterPath = Path.Combine(screenFileInfo.Directory.FullName, $"{presenterName}.cs");
			FileInfo presenterFileInfo = new FileInfo(presenterPath);

			ClassFileWriter.ClassDefinition classDefinition = new ClassFileWriter.ClassDefinition
			{
				Imports = new List<string> {"Muui"},
				Name = presenterName,
				Superclass = "BasePresenter",
				SuperclassGenerics = new List<string> {screenName}
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

			ClassFileWriter.MethodDefinition onPropertiesSetDefinition = new ClassFileWriter.MethodDefinition
			{
				VisibilityModifier = ClassFileWriter.VisibilityModifier.Protected,
				Modifier = ClassFileWriter.MethodDefinition.MethodModifier.Override,
				ReturnType = "void",
				Name = "OnPropertiesSet",
				Body = "// Subscribe to CurrentProperties change events here"
			};

			ClassFileWriter.MethodDefinition unsubscribeFromPropertiesDefinition = new ClassFileWriter.MethodDefinition
			{
				VisibilityModifier = ClassFileWriter.VisibilityModifier.Protected,
				Modifier = ClassFileWriter.MethodDefinition.MethodModifier.Override,
				ReturnType = "void",
				Name = "UnsubscribeFromProperties",
				Body = "// Unsubscribe from CurrentProperties change events here"
			};

			ClassFileWriter.ClassDefinition propertiesDefinition = new ClassFileWriter.ClassDefinition
			{
				Attributes = new List<string> {"Serializable"},
				Name = "Properties",
				Superclass = "PanelProperties",
				AdditionalBody = "// Add your custom properties here"
			};

			ClassFileWriter.ClassDefinition classDefinition = new ClassFileWriter.ClassDefinition
			{
				Imports = new List<string> {"Muui", "System"},
				Name = panelName,
				Superclass = "BasePanelController",
				SuperclassGenerics = new List<string> {$"{panelName}.Properties"},
				InnerClasses = new List<ClassFileWriter.ClassDefinition>{propertiesDefinition},
				Methods = new List<ClassFileWriter.MethodDefinition>
				{
					onPropertiesSetDefinition,
					unsubscribeFromPropertiesDefinition
				}
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

		private static void CreateWindowWithoutPropertiesFile(string path)
		{
			FileInfo windowFileInfo = new FileInfo(path);

			if (windowFileInfo.Exists)
			{
				Debug.LogError("The file already exists! Select a new file name.");
			}
			else
			{
				WriteWindowWithoutPropertiesFile(windowFileInfo);
				WriteScreenPresenterWithoutPropertiesFile(windowFileInfo);
				AssetDatabase.Refresh();
			}
		}

		private static void WriteWindowWithoutPropertiesFile(FileInfo windowFileInfo)
		{
			string windowName = windowFileInfo.Name.Substring(0, windowFileInfo.Name.Length - 3);
			ClassFileWriter.ClassDefinition classDefinition = new ClassFileWriter.ClassDefinition
			{
				Imports = new List<string> {"Muui"},
				Name = windowName,
				Superclass = "BaseWindowController"
			};

			ClassFileWriter.WriteClassFile(windowFileInfo, classDefinition);
		}

		private static void CreateWindowWithPropertiesFile(string path)
		{
			FileInfo windowFileInfo = new FileInfo(path);

			if (windowFileInfo.Exists)
			{
				Debug.LogError("The file already exists! Select a new file name.");
			}
			else
			{
				WriteWindowWithPropertiesFile(windowFileInfo);
				WritePanelPresenterWithPropertiesFile(windowFileInfo);
				AssetDatabase.Refresh();
			}
		}

		private static void WriteWindowWithPropertiesFile(FileInfo windowFileInfo)
		{
			string windowName = windowFileInfo.Name.Substring(0, windowFileInfo.Name.Length - 3);

			ClassFileWriter.MethodDefinition onPropertiesSetDefinition = new ClassFileWriter.MethodDefinition
			{
				VisibilityModifier = ClassFileWriter.VisibilityModifier.Protected,
				Modifier = ClassFileWriter.MethodDefinition.MethodModifier.Override,
				ReturnType = "void",
				Name = "OnPropertiesSet",
				Body = "// Subscribe to CurrentProperties change events here"
			};

			ClassFileWriter.MethodDefinition unsubscribeFromPropertiesDefinition = new ClassFileWriter.MethodDefinition
			{
				VisibilityModifier = ClassFileWriter.VisibilityModifier.Protected,
				Modifier = ClassFileWriter.MethodDefinition.MethodModifier.Override,
				ReturnType = "void",
				Name = "UnsubscribeFromProperties",
				Body = "// Unsubscribe from CurrentProperties change events here"
			};

			ClassFileWriter.ClassDefinition propertiesDefinition = new ClassFileWriter.ClassDefinition
			{
				Attributes = new List<string> {"Serializable"},
				Name = "Properties",
				Superclass = "WindowProperties",
				AdditionalBody = "// Add your custom properties here"
			};

			ClassFileWriter.ClassDefinition classDefinition = new ClassFileWriter.ClassDefinition
			{
				Imports = new List<string> {"Muui", "System"},
				Name = windowName,
				Superclass = "BaseWindowController",
				SuperclassGenerics = new List<string> {$"{windowName}.Properties"},
				InnerClasses = new List<ClassFileWriter.ClassDefinition>{propertiesDefinition},
				Methods = new List<ClassFileWriter.MethodDefinition>
				{
					onPropertiesSetDefinition,
					unsubscribeFromPropertiesDefinition
				}
			};

			ClassFileWriter.WriteClassFile(windowFileInfo, classDefinition);
		}
	}
}
