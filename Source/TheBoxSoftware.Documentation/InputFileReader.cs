using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;

namespace TheBoxSoftware.Documentation
{
    /// <summary>
    /// Class that reads solutions, projects and libraries and converts them in to
    /// DocumentedAssembly lists.
    /// </summary>
    /// <remarks>
    /// <see cref="Document"/>s are collections of DocumentedAssembly files, this class
    /// parses input files and returns the correctly instantiated DocumentAssembly instances.
    /// </remarks>
    /// <example>
    /// The filename and build configuration are required.
    /// <code>
    /// List&lt;DocumentedAssembly&gt; assemblies = InputFileReader.Read(
    ///     "c:\projects\mysolution.sln", "Debug"
    ///     );
    /// Document doc = new Document(assemblies);
    /// </code>
    /// </example>
    /// <seealso cref="Document"/>
    /// <seealso cref="DocumentedAssembly"/>
    public static class InputFileReader
    {
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
        /// <exception cref="ArgumentException">The filename is not an accepted extension</exception>
        public static List<DocumentedAssembly> Read(string fileName, string buildConfiguration)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            List<DocumentedAssembly> files = null;
            FileReader reader = null;

            switch (Path.GetExtension(fileName).ToLower())
            {
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
                default:
                    throw new ArgumentException("Provided filename is for a non valid file type", fileName);
            }

            reader.BuildConfiguration = string.IsNullOrEmpty(buildConfiguration) ? "Debug" : buildConfiguration;
            files = reader.Read();
            return files;
        }

        #region File Parsers
        /// <summary>
        /// Base class that provides methods and properties for reading different
        /// files.
        /// </summary>
        private abstract class FileReader
        {
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
            /// The build configuration to use when searching project and solution files.
            /// </summary>
            public string BuildConfiguration { get; set; }

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
        private class SolutionFileReader : FileReader
        {
            private const string VersionPattern = @"Microsoft Visual Studio Solution File, Format Version ([\d\.]*)";
            private const string V10ProjectPattern = "Project.*\\\".*\\\".*\\\".*\\\".*\\\"(.*)\\\".*\\\".*\\\"";
            private string[] ValidExtensions = new string[] { ".csproj", ".vbproj", ".vcproj" };

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
            public override List<DocumentedAssembly> Read()
            {
                string solutionFile = File.ReadAllText(this.FileName);
                List<string> projectFiles = new List<string>();
                List<DocumentedAssembly> references = new List<DocumentedAssembly>();

                // Find the version number
                Match versionMatch = Regex.Match(solutionFile, VersionPattern);

                // Find all the project files
                MatchCollection projectFileMatches = Regex.Matches(solutionFile, V10ProjectPattern);
                foreach (Match current in projectFileMatches)
                {
                    if (current.Groups.Count == 2) {
                        string projectFile = current.Groups[1].Value;
                        if (ValidExtensions.Contains(System.IO.Path.GetExtension(projectFile))) {
                            projectFiles.Add(projectFile);
                        }
                    }
                }

                foreach (string project in projectFiles)
                {
                    string fullProjectPath = System.IO.Path.GetDirectoryName(this.FileName) + "\\" + project;
                    if (System.IO.File.Exists(fullProjectPath))
                    {
                        ProjectFileReader reader = ProjectFileReader.Create(fullProjectPath);
                        reader.BuildConfiguration = this.BuildConfiguration;
                        references.AddRange(reader.Read());
                    }
                }

                return references;
            }

            #region Properties
            /// <summary>
            /// The visual studio solution file version
            /// </summary>
            public string Version
            {
                get;
                set;
            }
            #endregion
        }

        /// <summary>
        /// Reads a project and returns all the reference project library files.
        /// </summary>
        private abstract class ProjectFileReader : FileReader
        {
            /// <summary>
            /// Initialises a new instance of the ProjectFileReader class.
            /// </summary>
            /// <param name="fileName">The full path of the project file to read.</param>
            public ProjectFileReader(string fileName) : base(fileName) { }

            /// <summary>
            /// Factory method for instantiating <see cref="ProjectFileReader" /> instances.
            /// </summary>
            /// <param name="filename">The name of the project file to read.</param>
            /// <returns>An instance of a ProjectFileReader.</returns>
            /// <remarks>
            /// This method automatically determines which ProjectFileReader to instantiate
            /// from the file provided.
            /// </remarks>
            /// <exception cref="ArgumentNullException">
            /// Thrown when the provided <paramref name="filename"/> is null or empty.
            /// </exception>
            public static ProjectFileReader Create(string filename)
            {
                if (string.IsNullOrEmpty(filename))
                    throw new ArgumentNullException("filename");

                XmlDocument doc = new XmlDocument();
                doc.Load(filename);

                // should find a nice way of figuring out the schema version numbers and loading a reader based on that
                // but speed is of the essance! [#94]
                if (doc.FirstChild.Name == "Project" || (doc.FirstChild.Name == "xml" && doc.FirstChild.NextSibling.Name == "Project"))
                {
                    return new VS2005ProjectFileReader(filename);
                }
                else
                {
                    return new VS2003ProjectFileReader(filename);
                }
            }

            /// <summary>
            /// Reads the contents of the project file and returns the details of the
            /// output assembly for this project
            /// </summary>
            /// <returns>An array of assembly files, although there will only ever be one from here.</returns>
            public override List<DocumentedAssembly> Read()
            {
                ProjectFileProperties properties = this.ParseProject();

                if (!string.IsNullOrEmpty(properties.OutputPath))
                {
                    string outputFile = string.Format(@"{0}\{1}{2}.{3}",
                                    System.IO.Path.GetDirectoryName(this.FileName),
                                    properties.OutputPath,
                                    properties.LibraryName,
                                    properties.GetOutputExtension());

                    string documentation = string.Empty;
                    if (!string.IsNullOrEmpty(properties.DocumentationFile))
                    {
                        if (System.IO.Path.IsPathRooted(properties.DocumentationFile))
                        {
                            documentation = properties.DocumentationFile;
                        }
                        else
                        {
                            // We need to check if the file is relative to the project file or simply output
                            // in the output directory
                            documentation = System.IO.Path.GetFullPath(System.IO.Path.GetDirectoryName(this.FileName) + "\\" + properties.DocumentationFile);
                            if (!System.IO.File.Exists(documentation))
                            {
                                documentation = System.IO.Path.GetFullPath(System.IO.Path.GetDirectoryName(this.FileName) + "\\" + properties.OutputPath + "\\" + properties.DocumentationFile);
                            }
                        }
                    }

                    return new List<DocumentedAssembly>() {
                        new DocumentedAssembly(outputFile, documentation)
                        };
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            /// <summary>
            /// Parses the contents of the <see cref="FileReader.FileName"/> and returns the
            /// details to the caller.
            /// </summary>
            /// <returns>The relevant properties from the project files.</returns>
            protected abstract ProjectFileProperties ParseProject();

            /// <summary>
            /// A data class that contains all the required information from the project
            /// files.
            /// </summary>
            protected struct ProjectFileProperties
            {
                /// <summary>
                /// The type of output Library, etc.
                /// </summary>
                public string OutputType { get; set; }

                /// <summary>
                /// The name of the library
                /// </summary>
                public string LibraryName { get; set; }

                /// <summary>
                /// The path the library will be output to
                /// </summary>
                public string OutputPath { get; set; }

                /// <summary>
                /// The filename for the associated documentation file.
                /// </summary>
                public string DocumentationFile { get; set; }

                /// <summary>
                /// Calculates and returns the output extension depending on the <see cref="OutputType"/>.
                /// </summary>
                /// <returns>The correct file extension (only exe not .exe).</returns>
                public string GetOutputExtension()
                {
                    string extension = string.Empty;
                    switch (this.OutputType.ToLower())
                    {
                        case "library":
                            extension = "dll";
                            break;
                        case "winexe":
                        case "exe":
                            extension = "exe";
                            break;
                        default:
                            break;
                    }
                    return extension;
                }
            }
        }

        /// <summary>
        /// Profile file reader that can be used to read the project files from Visual
        /// Studio 2005, 2008 and 2010.
        /// </summary>
        /// <seealso cref="VS2003ProjectFileReader"/>
        private class VS2005ProjectFileReader : ProjectFileReader
        {
            /// <summary>
            /// Initialises a new instance of the VS2003 project file reader.
            /// </summary>
            /// <param name="filename">The filename of the project</param>
            public VS2005ProjectFileReader(string filename) :
                base(filename)
            {
            }

            /// <summary>
            /// Parses the contents of the <see cref="FileName"/> and returns the
            /// details to the caller.
            /// </summary>
            /// <returns>The relevant properties from the project files.</returns>
            protected override ProjectFileProperties ParseProject()
            {
                XmlDocument projectFile = new XmlDocument();
                projectFile.Load(this.FileName);

                XmlNamespaceManager namespaceManager = new XmlNamespaceManager(projectFile.NameTable);
                XmlNode projectNode = null;
                foreach (XmlNode topChild in projectFile)
                {
                    if (topChild.Name == "Project")
                    {
                        projectNode = topChild;
                    }
                }
                namespaceManager.AddNamespace("pr", projectNode.NamespaceURI);
                // DocumentationFile
                XmlNode assemblyNode = projectFile.SelectSingleNode(@"/pr:Project/pr:PropertyGroup/pr:AssemblyName", namespaceManager);
                XmlNode outputTypeNode = projectFile.SelectSingleNode(@"/pr:Project/pr:PropertyGroup/pr:OutputType", namespaceManager);
                XmlNodeList conditionalGroups = projectFile.SelectNodes(@"/pr:Project/pr:PropertyGroup[@Condition]", namespaceManager);
                XmlNodeList ouputPathNodes = projectFile.SelectNodes(@"/pr:Project/pr:PropertyGroup[@Condition]/pr:OutputPath", namespaceManager);
                XmlNode parentPropertyGroup = assemblyNode.ParentNode;

                string outputExtension = string.Empty;
                string libraryName = assemblyNode.InnerText;
                string outputPath = string.Empty;
                string documentationPath = string.Empty;

                foreach (XmlNode currentNode in conditionalGroups)
                {
                    foreach (XmlAttribute attribute in currentNode.Attributes)
                    {
                        if (attribute.Name == "Condition" && (attribute.Value.IndexOf(this.BuildConfiguration, StringComparison.InvariantCultureIgnoreCase) != -1)) {
                            XmlNode outPath = currentNode.SelectSingleNode("pr:OutputPath", namespaceManager);
                            XmlNode docPath = currentNode.SelectSingleNode("pr:DocumentationFile", namespaceManager);

                            if (outPath != null)
                                outputPath = outPath.InnerText;
                            if (docPath != null)
                                documentationPath = docPath.InnerText;
                        }
                    }
                }

                ProjectFileProperties properties = new ProjectFileProperties();
                properties.OutputType = outputTypeNode.InnerText;
                properties.LibraryName = libraryName;
                properties.OutputPath = outputPath;
                properties.DocumentationFile = documentationPath;
                return properties;
            }
        }

        /// <summary>
        /// Profile file reader that can be used to read the project files from Visual
        /// Studio 2002 & 2003
        /// </summary>
        /// <seealso cref="VS2005ProjectFileReader"/>
        private class VS2003ProjectFileReader : ProjectFileReader
        {
            /// <summary>
            /// Initialises a new instance of the VS2003 project file reader.
            /// </summary>
            /// <param name="filename">The filename of the project</param>
            public VS2003ProjectFileReader(string filename) :
                base(filename)
            {
            }

            protected override ProjectFileReader.ProjectFileProperties ParseProject()
            {
                XmlDocument projectFile = new XmlDocument();
                projectFile.Load(this.FileName);

                XmlNamespaceManager namespaceManager = new XmlNamespaceManager(projectFile.NameTable);
                XmlNode projectNode = null;
                foreach (XmlNode topChild in projectFile)
                {
                    if (topChild.Name == "VisualStudioProject")
                    {
                        projectNode = topChild;
                    }
                }

                // DocumentationFile
                XmlNode settings = projectFile.SelectSingleNode(@"/VisualStudioProject/*/Build/Settings");
                XmlNode debugNode = projectFile.SelectSingleNode(@"/VisualStudioProject/*/Build/Settings/Config[@Name='" +
                        this.BuildConfiguration + "']");

                string outputExtension = string.Empty;
                string libraryName = settings.Attributes["AssemblyName"].Value;
                string outputPath = debugNode.Attributes["OutputPath"].Value;
                string documentationFile = string.Empty;

                if (debugNode.Attributes["DocumentationFile"] != null) {
                    documentationFile = debugNode.Attributes["DocumentationFile"].Value;
                }

                ProjectFileProperties properties = new ProjectFileProperties();
                properties.OutputType = settings.Attributes["OutputType"].Value;
                properties.LibraryName = libraryName;
                properties.OutputPath = outputPath;
                properties.DocumentationFile = documentationFile;
                return properties;
            }
        }

        /// <summary>
        /// Reads a single file, dll or exe and returns the filename
        /// </summary>
        private class LibraryFileReader : FileReader
        {
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
            public override List<DocumentedAssembly> Read()
            {
                return new List<DocumentedAssembly>() { new DocumentedAssembly(this.FileName) };
            }
        }
        #endregion
    }
}