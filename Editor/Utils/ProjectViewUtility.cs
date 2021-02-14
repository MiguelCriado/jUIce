using System.IO;
using UnityEditor;
using UnityEngine;

namespace Juice.Editor
{
	public class ProjectViewUtility
	{
		public static string GetSelectedPathOrFallback(string fallback)
		{
			string result = fallback;

			foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
			{
				result = AssetDatabase.GetAssetPath(obj);

				if (!string.IsNullOrEmpty(result) && File.Exists(result))
				{
					result = Path.GetDirectoryName(result);
					break;
				}
			}
			return result;
		}

		public static string GetSelectedPathOrFallback()
		{
			return GetSelectedPathOrFallback("Assets");
		}
	}
}
