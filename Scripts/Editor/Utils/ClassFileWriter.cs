using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Juice.Editor
{
	public static class ClassFileWriter
	{
		public enum VisibilityModifier
		{
			Public,
			Protected,
			Private,
			Internal
		}

		public class ClassDefinition
		{
			public List<string> Imports;
			public string Namespace;
			public List<string> Attributes;
			public VisibilityModifier VisibilityModifier;
			public string Name;
			public List<string> ClassGenerics;
			public string Superclass;
			public List<string> SuperclassGenerics;
			public List<ClassDefinition> InnerClasses;
			public List<MethodDefinition> Methods;
			public string AdditionalBody;
		}

		public class MethodDefinition
		{
			public enum MethodModifier
			{
				None,
				Virtual,
				Override,
				New,
			}

			public List<string> Attributes;
			public VisibilityModifier VisibilityModifier;
			public bool IsStatic;
			public MethodModifier Modifier;
			public string ReturnType;
			public string Name;
			public List<string> Generics;
			public List<ParameterDefinition> Parameters;
			public string Body;
		}

		public class ParameterDefinition
		{
			public string Type;
			public string Name;
		}

		private static StringBuilder Builder = new StringBuilder();

		public static void WriteClassFile(FileInfo fileInfo, ClassDefinition definition)
		{
			using (StreamWriter writer = new StreamWriter(fileInfo.FullName))
			{
				int indentLevel = 0;

				WriteImports(writer, indentLevel, definition.Imports, definition.Namespace);
				WriteClass(writer, indentLevel, definition);
				WriteLine(writer, indentLevel, "");
			}
		}

		private static void WriteImports(StreamWriter writer, int indentLevel, List<string> imports, string namespaceName)
		{
			if (imports != null)
			{
				List<string> validImports = new List<string>(imports);
				validImports.Remove(namespaceName);

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

		private static void WriteClass(StreamWriter writer, int indentLevel, ClassDefinition definition)
		{
			if (definition != null)
			{
				indentLevel = WriteNamespaceHeader(writer, indentLevel, definition.Namespace);
				WriteAttributes(writer, indentLevel, definition.Attributes);
				WriteClassHeader(writer, indentLevel, definition);
				WriteLine(writer, indentLevel, "{");
				indentLevel++;
				WriteClassBody(writer, indentLevel, definition);
				indentLevel--;
				WriteLine(writer, indentLevel, "}");
				WriteNamespaceClosingBracket(writer, indentLevel, definition.Namespace);
			}
		}

		private static int WriteNamespaceHeader(StreamWriter writer, int indentLevel, string namespaceName)
		{
			bool isNamespaceSet = string.IsNullOrEmpty(namespaceName) == false;

			if (isNamespaceSet)
			{
				WriteLine(writer, indentLevel, $"namespace {namespaceName}");
				WriteLine(writer, indentLevel, "{");
				indentLevel++;
			}

			return indentLevel;
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
			string visibility = definition.VisibilityModifier.ToString().ToLower();
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
			WriteInnerClasses(writer, indentLevel, definition);
			WriteMethods(writer, indentLevel, definition);
			WriteBody(writer, indentLevel, definition.AdditionalBody);

			bool hasInnerClasses = definition.InnerClasses != null && definition.InnerClasses.Count > 0;
			bool hasMethods = definition.Methods != null && definition.Methods.Count > 0;
			bool hasAdditionalBody = string.IsNullOrEmpty(definition.AdditionalBody) == false;

			if (!hasInnerClasses && !hasMethods && !hasAdditionalBody)
			{
				writer.WriteLine();
			}
		}

		private static void WriteInnerClasses(StreamWriter writer, int indentLevel, ClassDefinition definition)
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
		}

		private static void WriteMethods(StreamWriter writer, int indentLevel, ClassDefinition definition)
		{
			bool hasMethods = definition.Methods != null && definition.Methods.Count > 0;

			if (hasMethods)
			{
				if (definition.InnerClasses != null && definition.InnerClasses.Count > 0)
				{
					writer.WriteLine();
				}

				for (int i = 0; i < definition.Methods.Count; i++)
				{
					MethodDefinition method = definition.Methods[i];
					WriteMethod(writer, indentLevel, method);

					if (i < definition.Methods.Count - 1)
					{
						writer.WriteLine();
					}
				}
			}
		}

		private static void WriteBody(StreamWriter writer, int indentLevel, string body)
		{
			if (string.IsNullOrEmpty(body) == false)
			{
				string[] bodyLines = body.Split('\n');

				foreach (string line in bodyLines)
				{
					string trimmedLine = line.Trim(' ', '\t');
					WriteLine(writer, indentLevel, trimmedLine);
				}
			}
		}

		private static void WriteMethod(StreamWriter writer, int indentLevel, MethodDefinition method)
		{
			WriteAttributes(writer, indentLevel, method.Attributes);
			WriteMethodHeader(writer, indentLevel, method);
			indentLevel++;
			WriteMethodBody(writer, indentLevel, method);
			indentLevel--;
			WriteLine(writer, indentLevel, "}");
		}

		private static void WriteMethodHeader(StreamWriter writer, int indentLevel, MethodDefinition method)
		{
			writer.Write(GetIndentation(indentLevel));
			writer.Write(method.VisibilityModifier.ToString().ToLower());
			writer.Write(GetStatic(method.IsStatic));
			writer.Write(GetMethodModifier(method.Modifier));
			writer.Write(" ");
			writer.Write(method.ReturnType);
			writer.Write(" ");
			writer.Write(method.Name);
			writer.Write(GetMethodGenerics(method.Generics));
			writer.Write("(");
			writer.Write(GetMethodParameters(method.Parameters));
			writer.WriteLine(")");
			WriteLine(writer, indentLevel, "{");
		}

		private static string GetStatic(bool isStatic)
		{
			string result = string.Empty;

			if (isStatic)
			{
				result = " static";
			}

			return result;
		}

		private static string GetMethodModifier(MethodDefinition.MethodModifier modifier)
		{
			string result = string.Empty;

			if (modifier != MethodDefinition.MethodModifier.None)
			{
				result = $" {modifier.ToString().ToLower()}";
			}

			return result;
		}

		private static string GetMethodGenerics(List<string> generics)
		{
			Builder.Length = 0;

			if (generics != null && generics.Count > 0)
			{
				Builder.Append("<");

				for (int i = 0; i < generics.Count; i++)
				{
					Builder.Append(generics[i]);

					if (i < generics.Count - 1)
					{
						Builder.Append(", ");
					}
				}

				Builder.Append(">");
			}

			return Builder.ToString();
		}

		private static string GetMethodParameters(List<ParameterDefinition> parameters)
		{
			Builder.Length = 0;

			if (parameters != null && parameters.Count > 0)
			{
				for (int i = 0; i < parameters.Count; i++)
				{
					Builder.Append($"{parameters[i].Type} {parameters[i].Name}");

					if (i < parameters.Count - 1)
					{
						Builder.Append(", ");
					}
				}
			}

			return Builder.ToString();
		}

		private static void WriteMethodBody(StreamWriter writer, int indentLevel, MethodDefinition method)
		{
			WriteOverrideCallToBase(writer, indentLevel, method);
			WriteBody(writer, indentLevel, method.Body);
		}

		private static void WriteOverrideCallToBase(StreamWriter writer, int indentLevel, MethodDefinition method)
		{
			if (method.Modifier == MethodDefinition.MethodModifier.Override)
			{
				writer.Write(GetIndentation(indentLevel));
				writer.Write($"base.{method.Name}(");

				if (method.Parameters != null && method.Parameters.Count > 0)
				{
					for (int i = 0; i < method.Parameters.Count; i++)
					{
						writer.Write($"{method.Parameters[i].Name}");

						if (i < method.Parameters.Count - 1)
						{
							writer.Write(", ");
						}
					}
				}

				writer.WriteLine(");");
			}
		}

		private static int WriteNamespaceClosingBracket(StreamWriter writer, int indentLevel, string namespaceName)
		{
			bool isNamespaceSet = string.IsNullOrEmpty(namespaceName) == false;

			if (isNamespaceSet)
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
