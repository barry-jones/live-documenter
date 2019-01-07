
namespace TheBoxSoftware.Documentation
{
    using System.Xml;

    /// <summary>
    /// Profile file reader that can be used to read the project files from Visual
    /// Studio 2002 & 2003
    /// </summary>
    /// <seealso cref="VS2005ProjectFileReader"/>
    internal class VS2003ProjectFileReader : ProjectFileReader
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
}