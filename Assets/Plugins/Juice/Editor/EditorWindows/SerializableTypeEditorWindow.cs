using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Juice.Editor
{
	public class SerializableTypeEditorWindow : EditorWindow
	{
		private readonly struct MatchEntry
		{
			public readonly string Value;
			public readonly int Score;

			public MatchEntry(string value, int score)
			{
				Value = value;
				Score = score;
			}
		}

		private static readonly GUIStyle WindowStyle = new GUIStyle("window")
		{
			padding = new RectOffset(4, 4, 4, 4)
		};

		private static readonly GUIStyle SelectionStyle = new GUIStyle("OL SelectedRow");

		private IEnumerable<string> options;
		private Action<string> selectionCallback;
		private Vector2 scrollPos;

		private string searchTerm;

		public static void Show(
			Rect buttonRect,
			IEnumerable<string> options,
			Action<string> selectionCallback)
		{
			var window = CreateInstance<SerializableTypeEditorWindow>();
			window.options = options;
			window.selectionCallback = selectionCallback;

			Rect windowRect = buttonRect;
			windowRect.position = GUIUtility.GUIToScreenPoint(buttonRect.position);

			float width = buttonRect.width;
			float height = 200;
			Vector2 windowSize = new Vector2(width, height);

			window.ShowAsDropDown(windowRect, windowSize);
		}

		private void Update()
		{
			Repaint();
		}

		public void OnGUI()
		{
			string selection = null;

			using (var windowRect = new EditorGUILayout.VerticalScope(WindowStyle))
			{
				if (Event.current.type == EventType.Repaint)
				{
					WindowStyle.Draw(windowRect.rect, true, true, true, true);
				}

				GUIStyle toolBarStyle = new GUIStyle(EditorStyles.toolbar);

				using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
				{
					searchTerm = EditorGUILayout.TextField(searchTerm, EditorStyles.toolbarSearchField);
				}

				using (var scrollView = new EditorGUILayout.ScrollViewScope(scrollPos))
				{
					scrollPos = scrollView.scrollPosition;

					foreach (string current in GetFilteredOptions(searchTerm).Take(100))
					{
						Rect rowRect = EditorGUILayout.BeginHorizontal();

						if (rowRect.Contains(Event.current.mousePosition))
						{
							switch (Event.current.type)
							{
								case EventType.Repaint:
									SelectionStyle.Draw(rowRect, true, true, true, false);
									break;
								case EventType.MouseDown:
									selection = current;
									selectionCallback?.Invoke(selection);
									break;
							}
						}

						EditorGUILayout.LabelField(current);
						EditorGUILayout.EndHorizontal();
					}
				}
			}

			if (string.IsNullOrEmpty(selection) == false)
			{
				Close();
			}
		}

		private IEnumerable<string> GetFilteredOptions(string searchTerm)
		{
			IEnumerable<string> result = options;

			if (string.IsNullOrEmpty(searchTerm) == false)
			{
				List<MatchEntry> matchEntries = new List<MatchEntry>();

				foreach (string current in options)
				{
					if (FuzzyMatcher.FuzzyMatch(current, searchTerm, out int score))
					{
						matchEntries.Add(new MatchEntry(current, score));
					}
				}

				matchEntries.Sort((a, b) => b.Score.CompareTo(a.Score));
				result = matchEntries.Select(x => x.Value);
			}

			return result;
		}
	}
}
