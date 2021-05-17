using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Juice.Editor
{
	public static class JuiceEditorGuiUtility
	{
		public class Icons
		{
			private static readonly Dictionary<string, Texture> CachedIcons = new Dictionary<string,Texture>();

			public static Texture Unlink => GetIconTexture("unlink");

			private static Texture GetIconTexture(string name)
			{
				if (CachedIcons.TryGetValue(name, out Texture result) == false)
				{
					if (EditorGUIUtility.isProSkin)
					{
						name = $"d_{name}";
					}

					if (EditorGUIUtility.pixelsPerPoint > 1.0)
					{
						name = $"{name}@2x";
					}

					result = AssetDatabase.LoadAssetAtPath<Texture2D>($"{AssetDatabaseUtils.IconsPath}{name}.png");

					if (result)
					{
						CachedIcons[name] = result;
					}
				}

				return result;
			}
		}

		public class ContentIcons
		{
			public static GUIContent Unlink => new GUIContent(Icons.Unlink);
		}
	}
}
