

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
            properties.OutputPath = readOutputPath();
            properties.LibraryName = readLibraryName();
            properties.OutputType = readOutputType();
            return properties;
        }

        private string readOutputPath()
        {
            string outputPath = readNodeValue(@"/Project/OutputPath");
            string basePath = readNodeValue(@"/Project/BaseOutputPath");
            string targetFramework = readNodeValue(@"/Project/PropertyGroup/TargetFramework");

            if(string.IsNullOrEmpty(outputPath))
            {
                outputPath = @"bin\Debug\";
            }

            return Path.Combine(basePath, outputPath, targetFramework);
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
            if (null != node && !string.IsNullOrEmpty(node.Value))
            {
                value = node.Value;
            }
            return value;
        }
    }
}
