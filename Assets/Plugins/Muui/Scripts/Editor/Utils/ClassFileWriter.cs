using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;

namespace Muui.Editor
{
	public static class ClassFileWriter
	{
		public class ClassDefinition
		{
			public enum Visibility
			{
				Public,
				Protected,
				Private,
				Internal
			}

			public List<string> Imports;
			public List<string> Attributes;
			public Visibility ClassVisibility;
			public string Name;
			public List<string> ClassGenerics;
			public string Superclass;
			public List<string> SuperclassGenerics;
			public List<ClassDefinition> InnerClasses;
		}

		private static StringBuilder Builder = new StringBuilder();

		public static void WriteClassFile(FileInfo fileInfo, ClassDefinition definition)
		{
			using (StreamWriter writer = new StreamWriter(fileInfo.FullName))
			{
				int indentLevel = 0;

				WriteImports(writer, indentLevel, definition.Imports);
				indentLevel = WriteNamespaceHeader(writer, indentLevel);
				WriteClass(writer, indentLevel, definition);
				indentLevel = WriteNamespaceClosingBracket(writer, indentLevel);
				WriteLine(writer, indentLevel, "");
			}
		}

		private static void WriteImports(StreamWriter writer, int indentLevel, List<string> imports)
		{
			if (imports != null)
			{
				List<string> validImports = new List<string>(imports);
				validImports.Remove(EditorSettings.projectGenerationRootNamespace);

				if (validImports.Count > 0)
				{
					foreach (string importClass in validImports)
					{
						WriteLine(writer, indentLevel, $"using {importClass};");
					}

					WriteLine(writer, indentLevel, "");
				}
			}
		}

		private static int WriteNamespaceHeader(StreamWriter writer, int indentLevel)
		{
			bool isRootNamespaceSet = string.IsNullOrEmpty(EditorSettings.projectGenerationRootNamespace) == false;

			if (isRootNamespaceSet)
			{
				WriteLine(writer, indentLevel, $"namespace {EditorSettings.projectGenerationRootNamespace}");
				WriteLine(writer, indentLevel, "{");
				indentLevel++;
			}

			return indentLevel;
		}

		private static void WriteClass(StreamWriter writer, int indentLevel, ClassDefinition definition)
		{
			if (definition != null)
			{
				WriteAttributes(writer, indentLevel, definition.Attributes);
				WriteClassHeader(writer, indentLevel, definition);
				WriteLine(writer, indentLevel, "{");
				indentLevel++;
				WriteClassBody(writer, indentLevel, definition);
				indentLevel--;
				WriteLine(writer, indentLevel, "}");
			}
		}

		private static void WriteAttributes(StreamWriter writer, int indentLevel, List<string> attributes)
		{
			if (attributes != null)
			{
				foreach (string attribute in attributes)
				{
					WriteLine(writer, indentLevel, $"[{attribute}]");
				}
			}
		}

		private static void WriteClassHeader(StreamWriter writer, int indentLevel, ClassDefinition definition)
		{
			string indentation = GetIndentation(indentLevel);
			string visibility = definition.ClassVisibility.ToString().ToLower();
			writer.Write($"{indentation}{visibility} class {definition.Name}");
			WriteGenericsList(writer, definition.ClassGenerics);
			WriteSuperclass(writer, definition.Superclass, definition.SuperclassGenerics);
		}

		private static void WriteSuperclass(StreamWriter writer, string superclass, List<string> superclassGenerics)
		{
			if (string.IsNullOrEmpty(superclass) == false)
			{
				writer.Write($" : {superclass}");
				WriteGenericsList(writer, superclassGenerics);
			}

			writer.WriteLine();
		}

		private static void WriteGenericsList(StreamWriter writer, List<string> genericsList)
		{
			if (genericsList != null && genericsList.Count > 0)
			{
				writer.Write("<");

				for (int i = 0; i < genericsList.Count; i++)
				{
					writer.Write(genericsList[i]);

					if (i < genericsList.Count - 1)
					{
						writer.Write(", ");
					}
				}

				writer.Write(">");
			}
		}

		private static void WriteClassBody(StreamWriter writer, int indentLevel, ClassDefinition definition)
		{
			var hasInnerClasses = WriteInnerClasses(writer, indentLevel, definition);

			if (hasInnerClasses == false)
			{
				WriteLine(writer, 0, "");
			}
		}

		private static bool WriteInnerClasses(StreamWriter writer, int indentLevel, ClassDefinition definition)
		{
			bool hasInnerClasses = definition.InnerClasses != null && definition.InnerClasses.Count > 0;

			if (hasInnerClasses)
			{
				for (int i = 0; i < definition.InnerClasses.Count; i++)
				{
					WriteClass(writer, indentLevel, definition.InnerClasses[i]);

					if (i < definition.InnerClasses.Count - 1)
					{
						WriteLine(writer, indentLevel, "");
					}
				}
			}

			return hasInnerClasses;
		}

		private static int WriteNamespaceClosingBracket(StreamWriter writer, int indentLevel)
		{
			bool isRootNamespaceSet = string.IsNullOrEmpty(EditorSettings.projectGenerationRootNamespace) == false;

			if (isRootNamespaceSet)
			{
				indentLevel--;
				WriteLine(writer, indentLevel, "}");
			}

			return indentLevel;
		}

		private static void WriteLine(StreamWriter writer, int indentLevel, string line)
		{
			writer.WriteLine($"{GetIndentation(indentLevel)}{line}");
		}

		private static string GetIndentation(int indentLevel)
		{
			Builder.Length = 0;

			for (int i = 0; i < indentLevel; i++)
			{
				Builder.Append("\t");
			}

			return Builder.ToString();
		}
}
}
