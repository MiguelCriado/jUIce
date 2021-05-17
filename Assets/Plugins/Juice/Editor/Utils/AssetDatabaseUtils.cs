using System.Linq;
using UnityEditor;

namespace Juice.Editor
{
	public static class AssetDatabaseUtils
	{
		public static string EditorPath => PathUp(FindAssetPath("Juice.Editor"));
		public static string IconsPath => $"{EditorPath}Icons/";

		public static string FindAssetPath(string assetName)
		{
			string guid = AssetDatabase.FindAssets(assetName).SingleOrDefault();
			return AssetDatabase.GUIDToAssetPath(guid);
		}

		public static string PathUp(string path, int numLevels = 1)
		{
			string result = path;

			if (numLevels > 0)
			{
				int lastSeparatorIndex = path.TrimEnd('/').LastIndexOf('/');

				if (lastSeparatorIndex >= 0)
				{
					result = path.Remove(lastSeparatorIndex + 1);
				}

				result = PathUp(result, numLevels - 1);
			}


			return result;
		}
	}
}
