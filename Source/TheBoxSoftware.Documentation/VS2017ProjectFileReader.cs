

namespace TheBoxSoftware.Documentation
{
    using System;
    using System.IO;
    using System.Xml;

    internal class VS2017ProjectFileReader : ProjectFileReader
    {
        private readonly XmlDocument _document;

        public VS2017ProjectFileReader(XmlDocument document, string filename) : base(filename)
        {
            _document = document;
        }

        internal override ProjectFileProperties ParseProject()
        {
            ProjectFileProperties properties = new ProjectFileProperties();
            properties.DocumentationFile = readDocumentationFile();
            properties.LibraryName = readLibraryName();
            properties.OutputType = readOutputType();

            properties.OutputPath = readOutputPath();

            return properties;
        }

        private string readOutputPath()
        {
            string outputPath = readNodeValue(@"/Project/PropertyGroup/OutputPath");
            string basePath = readNodeValue(@"/Project/PropertyGroup/BaseOutputPath");
            string targetFramework = readNodeValue(@"/Project/PropertyGroup/TargetFramework");
            string startOfPath = string.Empty;

            if(!string.IsNullOrEmpty(outputPath))
            {
                // base path is ignored, as is the current build configuration
                startOfPath = outputPath;
            }
            else if(!string.IsNullOrEmpty(basePath))
            {
                // base path and build configuration is used
                startOfPath = Path.Combine(basePath, BuildConfiguration);
            }
            else
            {
                startOfPath = Path.Combine("bin", BuildConfiguration);
            }

            return $"{Path.Combine(startOfPath, targetFramework)}\\";
        }

        private string readOutputType()
        {
            return readNodeValue(@"/Project/OutputType");
        }

        private string readLibraryName()
        {
            string value = readNodeValue(@"/Project/AssemblyName");
            return string.IsNullOrEmpty(value) ? Path.GetFileNameWithoutExtension(FileName) : value;
        }

        private string readDocumentationFile()
        {
            return readNodeValue(@"/Project/DocumentationFile");
        }

        private string readNodeValue(string xpath)
        {
            XmlNode node = _document.SelectSingleNode(xpath);
            string value = string.Empty;
            if (null != node && !string.IsNullOrEmpty(node.InnerText))
            {
                value = node.InnerText;
            }
            return value;
        }
    }
}
