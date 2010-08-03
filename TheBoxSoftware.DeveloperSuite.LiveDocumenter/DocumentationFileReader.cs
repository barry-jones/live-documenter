using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter {
	/// <summary>
	/// Helper class for reading and parsing file types to get the referenced
	/// libraries.
	/// </summary>
	public static class DocumentationFileReader {
		/// <summary>
		/// Reads and parses the file and returns all of the associated library
		/// references
		/// </summary>
		/// <param name="fileName">The filename to read</param>
		/// <returns>An array of <see cref="DocumentedAssembly"/> instances that
		/// represent the assemblies to be documented by the application.</returns>
		/// <exception cref="ArgumentNullException">
		/// Thrown when the <paramref name="fileName"/> provided is null or an
		/// empty string.
		/// </exception>
		public static List<DocumentedAssembly> Read(string fileName) {
			if (TraceHelper.IsTraceEnabled) {
				TraceHelper.WriteLine("reading file: {0}", fileName);
				TraceHelper.Indent();
			}

			if (string.IsNullOrEmpty(fileName)) {
				throw new ArgumentNullException("fileName");
			}

			List<DocumentedAssembly> files = null;
			FileReader reader = null;

			switch (Path.GetExtension(fileName).ToLower()) {
				case ".sln":
					reader = new SolutionFileReader(fileName);
					break;

				case ".csproj":
				case ".vbproj":
				case ".vcproj":
					reader = ProjectFileReader.Create(fileName);
					break;

				case ".dll":
				case ".exe":
					reader = new LibraryFileReader(fileName);
					break;
			}

			files = reader.Read();

			TraceHelper.Unindent();

			return files;
		}

		#region File Parsers
		/// <summary>
		/// Base class that provides methods and properties for reading different
		/// files.
		/// </summary>
		private abstract class FileReader {
			/// <summary>
			/// Initialises a new instance of the FileReader class
			/// </summary>
			/// <param name="fileName">The filename of the file being read</param>
			protected FileReader(string fileName) { 
				this.FileName = fileName; 
			}

			/// <summary>
			/// The original file name that was read by this FileReader
			/// </summary>
			protected string FileName { get; set; }

			/// <summary>
			/// Reads the file to get all the referenced libraries for the documentor
			/// </summary>
			/// <returns>
			/// The list of assemblies that have been documented by this application.
			/// </returns>
			public abstract List<DocumentedAssembly> Read();
		}

		/// <summary>
		/// Reads a solution and returns the solution and its associated projects
		/// for referenced libraries and returns them.
		/// </summary>
		private class SolutionFileReader : FileReader {
			private const string VersionPattern = @"Microsoft Visual Studio Solution File, Format Version ([\d\.]*)";
			private const string V10ProjectPattern = "Project.*\\\".*\\\".*\\\".*\\\".*\\\"(.*)\\\".*\\\".*\\\"";
			private string[] ValidExtensions = new string[] {".csproj", ".vbproj", ".vcproj"};

			/// <summary>
			/// Initialises a new instance of the SolutionFileReader class.
			/// </summary>
			/// <param name="fileName">The filenname and path for the solution</param>
			public SolutionFileReader(string fileName)
				: base(fileName) {
			}

			/// <summary>
			/// Reads the contents of the solution and returns all of the solutions project
			/// file output assemblies.
			/// </summary>
			/// <returns>An array of assemblies output by the solution and its projects.</returns>
			public override List<DocumentedAssembly> Read() {
				string solutionFile = File.ReadAllText(this.FileName);
				List<string> projectFiles = new List<string>();
				List<DocumentedAssembly> references = new List<DocumentedAssembly>();

				// Find the version number
				Match versionMatch = Regex.Match(solutionFile, VersionPattern);
				TraceHelper.WriteLine("solution version: {0}", versionMatch.Value);

				// Find all the project files
				MatchCollection projectFileMatches = Regex.Matches(solutionFile, V10ProjectPattern);
				TraceHelper.WriteLine("number of projects: {0}", projectFileMatches.Count);
				TraceHelper.Indent();
				foreach (Match current in projectFileMatches) {
					if (current.Groups.Count == 2) {
						string projectFile = current.Groups[1].Value;
						TraceHelper.WriteLine("project: {0}", projectFile);
						if (ValidExtensions.Contains(System.IO.Path.GetExtension(projectFile))) {
							projectFiles.Add(projectFile);
						}
					}
				}
				TraceHelper.Unindent();

				foreach (string project in projectFiles) {
					string fullProjectPath = System.IO.Path.GetDirectoryName(this.FileName) + "\\" + project;
					if (System.IO.File.Exists(fullProjectPath)) {
						ProjectFileReader reader = ProjectFileReader.Create(fullProjectPath);
						references.AddRange(reader.Read());
					}					
				}

				return references;
			}

			#region Properties
			/// <summary>
			/// The visual studio solution file version
			/// </summary>
			public string Version {
				get;
				set;
			}
			#endregion
		}

		/// <summary>
		/// Reads a project and returns all the reference project library files.
		/// </summary>
		private class ProjectFileReader : FileReader {
			/// <summary>
			/// Initialises a new instance of the ProjectFileReader class.
			/// </summary>
			/// <param name="fileName">The full path of the project file to read.</param>
			public ProjectFileReader(string fileName) : base(fileName) { }

			public static ProjectFileReader Create(string filename) {
				XmlDocument doc = new XmlDocument();
				doc.Load(filename);

				// should find a nice way of figuring out the schema version numbers and loading a reader based on that
				// but speed is of the essance! [#94]
				if (doc.FirstChild.Name == "Project" || (doc.FirstChild.Name=="xml" && doc.FirstChild.NextSibling.Name == "Project")) {
					TraceHelper.WriteLine("reading with 05 > ProjectFileReader");
					return new ProjectFileReader(filename);
				}
				else {
					TraceHelper.WriteLine("reading with 03 < VS2003ProjectFileReader");
					return new VS2003ProjectFileReader(filename);
				}
			}

			/// <summary>
			/// Reads the contents of the project file and returns the details of the
			/// output assembly for this project
			/// </summary>
			/// <returns>An array of assembly files, although there will only ever be one from here.</returns>
			public override List<DocumentedAssembly> Read() {
				// TODO: Consider how we will know which is the selected output type

				XmlDocument projectFile = new XmlDocument();
				projectFile.Load(this.FileName);

				XmlNamespaceManager namespaceManager = new XmlNamespaceManager(projectFile.NameTable);
				XmlNode projectNode = null;
				foreach (XmlNode topChild in projectFile) {
					if (topChild.Name == "Project") {
						projectNode = topChild;
					}
				}
				namespaceManager.AddNamespace("pr", projectNode.NamespaceURI);
				// DocumentationFile
				XmlNode assemblyNode = projectFile.SelectSingleNode(@"/pr:Project/pr:PropertyGroup/pr:AssemblyName", namespaceManager);
				XmlNode outputTypeNode = projectFile.SelectSingleNode(@"/pr:Project/pr:PropertyGroup/pr:OutputType", namespaceManager);
				XmlNodeList ouputPathNodes = projectFile.SelectNodes(@"/pr:Project/pr:PropertyGroup/pr:OutputPath", namespaceManager);
				XmlNode parentPropertyGroup = assemblyNode.ParentNode;

				string outputExtension = string.Empty;
				string libraryName = assemblyNode.InnerText;
				string outputPath = string.Empty;

				switch(outputTypeNode.InnerText) {
					case "Library":
						outputExtension = "dll";
						break;
					case "WinExe":
					case "Exe":
						outputExtension = "exe";
						break;
					default:
						int x = 0;
						break;
				}

				foreach (XmlNode currentNode in ouputPathNodes) {
					foreach (XmlAttribute attribute in currentNode.ParentNode.Attributes) {
						if (attribute.Name == "Condition") {
							if (attribute.Value.IndexOf(Model.UserApplicationStore.Store.Preferences.BuildConfiguration.ToString(), StringComparison.InvariantCultureIgnoreCase) != -1) {
								outputPath = currentNode.InnerText;
								break;
							}
						}
					}
				}

				if (!string.IsNullOrEmpty(outputPath)) {
					string outputFile = string.Format(@"{0}\{1}{2}.{3}",
							System.IO.Path.GetDirectoryName(this.FileName),
							outputPath,
							libraryName,
							outputExtension);
					if (System.IO.File.Exists(outputFile)) {
						return new List<DocumentedAssembly>() {
							new DocumentedAssembly(outputFile)
							};
					}
					else {
						return new List<DocumentedAssembly>();
					}
				}
				else {
					throw new InvalidOperationException();
				}
			}
		}

		private class VS2003ProjectFileReader : ProjectFileReader {
			/// <summary>
			/// Initialises a new instance of the VS2003 project file reader.
			/// </summary>
			/// <param name="filename">The filename of the project</param>
			public VS2003ProjectFileReader(string filename) : 
				base(filename) {
			}

			public override List<DocumentedAssembly> Read() {
								// TODO: Consider how we will know which is the selected output type

				XmlDocument projectFile = new XmlDocument();
				projectFile.Load(this.FileName);

				XmlNamespaceManager namespaceManager = new XmlNamespaceManager(projectFile.NameTable);
				XmlNode projectNode = null;
				foreach (XmlNode topChild in projectFile) {
					if (topChild.Name == "VisualStudioProject") {
						projectNode = topChild;
					}
				}

				// DocumentationFile
				XmlNode settings = projectFile.SelectSingleNode(@"/VisualStudioProject/*/Build/Settings");
				XmlNode debugNode = projectFile.SelectSingleNode(@"/VisualStudioProject/*/Build/Settings/Config[@Name='" + 
					Model.UserApplicationStore.Store.Preferences.BuildConfiguration.ToString() + "']");
				// XmlNodeList ouputPathNodes = projectFile.SelectNodes(@"/pr:Project/pr:PropertyGroup/pr:OutputPath", namespaceManager);
				// XmlNode parentPropertyGroup = assemblyNode.ParentNode;

				string outputExtension = string.Empty;
				string libraryName = settings.Attributes["AssemblyName"].Value;
				string outputPath = debugNode.Attributes["OutputPath"].Value;

				switch(settings.Attributes["OutputType"].Value) {
					case "Library":
						outputExtension = "dll";
						break;
					case "WinExe":
					case "Exe":
						outputExtension = "exe";
						break;
					default:
						int x = 0;
						break;
				}

				if (!string.IsNullOrEmpty(outputPath)) {
					string outputFile = string.Format(@"{0}\{1}{2}.{3}",
							System.IO.Path.GetDirectoryName(this.FileName),
							outputPath,
							libraryName,
							outputExtension);
					if (System.IO.File.Exists(outputFile)) {
						return new List<DocumentedAssembly>() {
							new DocumentedAssembly(outputFile)
							};
					}
					else {
						return new List<DocumentedAssembly>();
					}
				}
				else {
					throw new InvalidOperationException();
				}
			}
		}

		/// <summary>
		/// Reads a single file, dll or exe and returns the filename
		/// </summary>
		private class LibraryFileReader : FileReader {
			/// <summary>
			/// Initialises a new instance of the LibraryFileReader class.
			/// </summary>
			/// <param name="fileName">The full path and name of the assembly.</param>
			public LibraryFileReader(string fileName) : base(fileName) { }

			/// <summary>
			/// Reads the details of the assembly and its associated comment file
			/// form the FileName.
			/// </summary>
			/// <returns>The list of assemblies that are to be read by the application.</returns>
			public override List<DocumentedAssembly> Read() {
				return new List<DocumentedAssembly>() { new DocumentedAssembly(this.FileName) };
			}
		}
		#endregion
	}
}
