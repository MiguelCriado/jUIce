using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Juice.Utils
{
	public static class TransformExtensions
	{
		private static readonly StringBuilder builder = new StringBuilder();
		private static readonly List<Transform> pathList = new List<Transform>();

		public static List<Transform> GetPath(this Transform transform)
		{
			return GetPath(transform, new List<Transform>());
		}

		public static List<Transform> GetPath(this Transform transform, List<Transform> listToUse)
		{
			if (transform.parent)
			{
				transform.parent.GetPath(listToUse);
			}

			listToUse.Add(transform);

			return listToUse;
		}

		public static string PathToString(this Transform transform)
		{
			pathList.Clear();
			builder.Length = 0;

			List<Transform> path = transform.GetPath(pathList);

			for (int i = 0; i < path.Count; i++)
			{
				builder.Append(path[i].name);

				if (i < path.Count - 1)
				{
					builder.Append("/");
				}
			}

			return builder.ToString();
		}
	}
}
