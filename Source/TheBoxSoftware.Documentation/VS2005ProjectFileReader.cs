
namespace TheBoxSoftware.Documentation
{
    using System;
    using System.Xml;

    /// <summary>
    /// Profile file reader that can be used to read the project files from Visual
    /// Studio 2005, 2008 and 2010.
    /// </summary>
    /// <seealso cref="VS2003ProjectFileReader"/>
    internal class VS2005ProjectFileReader : ProjectFileReader
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
        internal override ProjectFileProperties ParseProject()
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
                    if (attribute.Name == "Condition" && (attribute.Value.IndexOf(this.BuildConfiguration, StringComparison.InvariantCultureIgnoreCase) != -1))
                    {
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
}