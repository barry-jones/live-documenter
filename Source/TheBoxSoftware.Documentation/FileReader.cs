
namespace TheBoxSoftware.Documentation
{
    using System.Collections.Generic;

    /// <summary>
    /// Base class that provides methods and properties for reading different
    /// files.
    /// </summary>
    internal abstract class FileReader
    {
        /// <summary>
        /// Initialises a new instance of the FileReader class
        /// </summary>
        /// <param name="fileName">The filename of the file being read</param>
        protected FileReader(string fileName)
        {
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
}