
namespace TheBoxSoftware.Documentation
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    /// <summary>
    /// Reads a project and returns all the reference project library files.
    /// </summary>
    internal abstract class ProjectFileReader : FileReader
    {
        /// <summary>
        /// Initialises a new instance of the ProjectFileReader class.
        /// </summary>
        /// <param name="fileName">The full path of the project file to read.</param>
        public ProjectFileReader(string fileName) : base(fileName) { }

        /// <summary>
        /// Factory method for instantiating <see cref="ProjectFileReader" /> instances.
        /// </summary>
        /// <include file='code-documentation\inputfilereader.xml' path='docs/projectfilereader/member[@name="Create"]/*' />
        public static ProjectFileReader Create(string filename, IFileSystem filesystem)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException("filename");

            string fileContent = filesystem.ReadAllText(filename);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(fileContent);

            // should find a nice way of figuring out the schema version numbers and loading a reader based on that
            // but speed is of the essance! [#94]
            if (doc.FirstChild.Name == "Project" && doc.FirstChild.Attributes["Sdk"] != null)
            {
                return new VS2017ProjectFileReader(doc, filename);
            }
            else if(doc.FirstChild.Name == "Project" || (doc.FirstChild.Name == "xml" && doc.FirstChild.NextSibling.Name == "Project"))
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
        internal abstract ProjectFileProperties ParseProject();

        /// <summary>
        /// A data class that contains all the required information from the project
        /// files.
        /// </summary>
        internal struct ProjectFileProperties
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
                switch (OutputType.ToLower())
                {
                    case "library":
                        extension = "dll";
                        break;
                    case "winexe":
                    case "exe":
                        extension = "exe";
                        break;
                    default:
                        extension = "dll";
                        break;
                }
                return extension;
            }
        }
    }
}